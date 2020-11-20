using Microsoft.Extensions.Logging;
using OrvilleX.Cache.Internal;
using OrvilleX.Cache.Util;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace OrvilleX.Cache
{
    /// <summary>
    /// 基于Redis的分布式锁
    /// </summary>
    public class RedisLock : AbstractLock
    {
        private readonly object _lockObject = new object();

        private readonly IRedisCache _redisCache;
        private readonly ILogger<RedisLock> _logger;

        private readonly int _quorumRetryCount;
        private readonly int _quorumRetryDelayMs;
        private readonly double _clockDriftFactor;
        private bool _isDisposed;

        private Timer _lockKeepaliveTimer;

        private static readonly string UnLockScript = EmbeddedResourceLoader.GetEmbeddedResource("Sino.Extensions.Redis.Lua.Unlock.lua");
        private static readonly string ExtendIfMatchingValueScript = EmbeddedResourceLoader.GetEmbeddedResource("Sino.Extensions.Redis.Lua.Extend.lua");

        public override bool IsAcquired => Status == LockStatus.Acquired;

        private readonly TimeSpan _expiryTime;
        private readonly TimeSpan? _waitTime;
        private readonly TimeSpan? _retryTime;
        private CancellationToken _cancellationToken;

        private readonly TimeSpan _minimumExpiryTime = TimeSpan.FromMilliseconds(10);
        private readonly TimeSpan _minimumRetryTime = TimeSpan.FromMilliseconds(10);

        private RedisLock(ILogger<RedisLock> logger, IRedisCache redisCache, string resource, TimeSpan expiryTime,
            TimeSpan? waitTime = null,
            TimeSpan? retryTime = null,
            CancellationToken? cancellationToken = null)
        {
            _logger = logger;

            if (expiryTime < _minimumExpiryTime)
            {
                logger.LogWarning($"Expiry time {expiryTime.TotalMilliseconds}ms too low, setting to {_minimumExpiryTime.TotalMilliseconds}ms");
                expiryTime = _minimumExpiryTime;
            }

            if (retryTime != null && retryTime.Value < _minimumRetryTime)
            {
                logger.LogWarning($"Retry time {retryTime.Value.TotalMilliseconds}ms too low, setting to {_minimumRetryTime.TotalMilliseconds}ms");
                retryTime = _minimumRetryTime;
            }

            _redisCache = redisCache;

            _quorumRetryCount = 3;
            _quorumRetryDelayMs = 400;
            _clockDriftFactor = 0.01;

            Resource = resource;
            LockId = Guid.NewGuid().ToString();
            _expiryTime = expiryTime;
            _waitTime = waitTime;
            _retryTime = retryTime;
            _cancellationToken = cancellationToken ?? CancellationToken.None;
        }

        public static RedisLock Create(ILogger<RedisLock> logger, IRedisCache redisCache, string resource, TimeSpan expiryTime,
            TimeSpan? waitTime = null,
            TimeSpan? retryTime = null,
            CancellationToken? cancellationToken = null)
        {
            var redisLock = new RedisLock(logger, redisCache, resource, expiryTime, waitTime, retryTime, cancellationToken);

            redisLock.Start();

            return redisLock;
        }

        public static async Task<RedisLock> CreateAsync(ILogger<RedisLock> logger, IRedisCache redisCache, string resource, TimeSpan expiryTime,
            TimeSpan? waitTime = null,
            TimeSpan? retryTime = null,
            CancellationToken? cancellationToken = null)
        {
            var redisLock = new RedisLock(logger, redisCache, resource, expiryTime, waitTime, retryTime, cancellationToken);

            await redisLock.StartAsync().ConfigureAwait(false);

            return redisLock;
        }

        private void Start()
        {
            if (_waitTime.HasValue && _retryTime.HasValue && _waitTime.Value.TotalMilliseconds > 0 && _retryTime.Value.TotalMilliseconds > 0)
            {
                var stopwatch = Stopwatch.StartNew();

                while(!IsAcquired && stopwatch.Elapsed <= _waitTime.Value)
                {
                    Status = Acquire();
                }

                if (!IsAcquired)
                {
                    Task.Delay(_retryTime.Value, _cancellationToken).Wait(_cancellationToken);
                }
            }
            else
            {
                Status = Acquire();
            }
            _logger.LogInformation($"Lock status: {Status}, {Resource} ({LockId})");

            if (IsAcquired)
            {
                StartAutoExtendTimer();
            }
        }

        private async Task StartAsync()
        {
            if (_waitTime.HasValue && _retryTime.HasValue && _waitTime.Value.TotalMilliseconds > 0 && _retryTime.Value.TotalMilliseconds > 0)
            {
                var stopwatch = Stopwatch.StartNew();

                while (!IsAcquired && stopwatch.Elapsed <= _waitTime.Value)
                {
                    Status = await AcquireAsync().ConfigureAwait(false);
                }

                if (!IsAcquired)
                {
                    Task.Delay(_retryTime.Value, _cancellationToken).Wait(_cancellationToken);
                }
            }
            else
            {
                Status = await AcquireAsync().ConfigureAwait(false);
            }
            _logger.LogInformation($"Lock status: {Status}, {Resource} ({LockId})");

            if (IsAcquired)
            {
                StartAutoExtendTimer();
            }
        }

        private LockStatus Acquire()
        {
            LockInstanceResult lockResult = LockInstanceResult.Error;

            for (var i = 0; i < _quorumRetryCount; i++)
            {
                _cancellationToken.ThrowIfCancellationRequested();

                var iteratio = i + 1;
                _logger.LogDebug($"Lock attempt {iteratio}/{_quorumRetryCount}: {Resource} ({LockId}), expiry: {_expiryTime}");

                var stopwatch = Stopwatch.StartNew();
                lockResult = Lock();

                var validityTicks = GetRemainingValidityTicks(stopwatch);

                _logger.LogDebug($"Acquired locks for {Resource} ({LockId}), validityTicks: {validityTicks}");

                if (lockResult == LockInstanceResult.Success && validityTicks > 0)
                {
                    return LockStatus.Acquired;
                }

                UnLock();

                if (i < _quorumRetryCount - 1)
                {
                    var sleepMs = ThreadSafeRandom.Next(_quorumRetryDelayMs);
                    _logger.LogDebug($"Sleeping {sleepMs}ms");

                    Task.Delay(sleepMs).Wait(_cancellationToken);
                }
            }

            var status = GetFailedLockStatus(lockResult);
            return status;
        }

        private async Task<LockStatus> AcquireAsync()
        {
            LockInstanceResult lockResult = LockInstanceResult.Error;

            for (var i = 0; i < _quorumRetryCount; i++)
            {
                _cancellationToken.ThrowIfCancellationRequested();

                var iteratio = i + 1;
                _logger.LogDebug($"Lock attempt {iteratio}/{_quorumRetryCount}: {Resource} ({LockId}), expiry: {_expiryTime}");

                var stopwatch = Stopwatch.StartNew();
                lockResult = await LockAsync().ConfigureAwait(false);

                var validityTicks = GetRemainingValidityTicks(stopwatch);

                _logger.LogDebug($"Acquired locks for {Resource} ({LockId}), validityTicks: {validityTicks}");

                if (lockResult == LockInstanceResult.Success && validityTicks > 0)
                {
                    return LockStatus.Acquired;
                }

                await UnLockAsync().ConfigureAwait(false);

                if (i < _quorumRetryCount - 1)
                {
                    var sleepMs = ThreadSafeRandom.Next(_quorumRetryDelayMs);
                    _logger.LogDebug($"Sleeping {sleepMs}ms");

                    await Task.Delay(sleepMs, _cancellationToken).ConfigureAwait(false);
                }
            }

            var status = GetFailedLockStatus(lockResult);
            return status;
        }

        private LockStatus GetFailedLockStatus(LockInstanceResult lockInstanceResult)
        {
            if (lockInstanceResult == LockInstanceResult.Conflicted)
            {
                return LockStatus.Conflicted;
            }
            else if (lockInstanceResult == LockInstanceResult.Success)
            {
                return LockStatus.Expired;
            }
            return LockStatus.NoQuorum;
        }

        private LockInstanceResult Lock()
        {
            LockInstanceResult result;

            try
            {
                _logger.LogTrace($"Lock: {Resource}, {LockId}, {_expiryTime}");
                var redisResult = _redisCache.SetNx(Resource, LockId, _expiryTime);

                result = redisResult ? LockInstanceResult.Success : LockInstanceResult.Conflicted;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error locking lock instance: {ex.Message}");

                result = LockInstanceResult.Error;
            }

            return result;
        }

        private bool UnLock()
        {
            var result = false;

            try
            {
                _logger.LogTrace($"UnLock: {Resource}, {LockId}");
                result = (bool)_redisCache.GetDatabase().ScriptEvaluate(UnLockScript, new RedisKey[] { Resource }, new RedisValue[] { LockId });
            }
            catch(Exception ex)
            {
                _logger.LogDebug($"Error unlocking lock instance: {ex.Message}");
            }

            _logger.LogTrace($"Unlock exit: {Resource}, {LockId}, {result}");

            return result;
        }

        private async Task<LockInstanceResult> LockAsync()
        {
            LockInstanceResult result;

            try
            {
                _logger.LogTrace($"LockAsync: {Resource}, {LockId}, {_expiryTime}");
                var redisResult = await _redisCache.SetNxAsync(Resource, LockId, _expiryTime).ConfigureAwait(false);

                result = redisResult ? LockInstanceResult.Success : LockInstanceResult.Conflicted;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error locking lock instance: {ex.Message}");
                result = LockInstanceResult.Error;
            }

            return result;
        }

        private async Task<bool> UnLockAsync()
        {
            var result = false;

            try
            {
                _logger.LogTrace($"UnLock: {Resource}, {LockId}");
                result = (bool)await _redisCache.GetDatabase().ScriptEvaluateAsync(UnLockScript, new RedisKey[] { Resource }, new RedisValue[] { LockId });
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error unlocking lock instance: {ex.Message}");
            }

            _logger.LogTrace($"Unlock exit: {Resource}, {LockId}, {result}");

            return result;
        }

        private void StartAutoExtendTimer()
        {
            var interval = _expiryTime.TotalMilliseconds / 2;

            _logger.LogDebug($"Starting auto extend timer with {interval}ms interval");

            _lockKeepaliveTimer = new Timer(
                state => 
                {
                    try
                    {
                        _logger.LogTrace($"Lock renewal timer fired: {Resource} ({LockId})");

                        var stopwatch = Stopwatch.StartNew();
                        var lockResult = Extend();
                        var validityTicks = GetRemainingValidityTicks(stopwatch);
                        if (lockResult == LockInstanceResult.Success && validityTicks > 0)
                        {
                            Status = LockStatus.Acquired;
                            ExtendCount++;

                            _logger.LogDebug($"Extended lock, {Status}: {Resource} ({LockId})");
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(null, ex, $"Lock renewal timer thread failed: {Resource} ({LockId})");
                    }
                },
                null,
                (int)interval,
                (int)interval);
        }

        private LockInstanceResult Extend()
        {
            LockInstanceResult result;

            try
            {
                _logger.LogTrace($"Extend enter: {Resource}, {LockId}, {_expiryTime}");

                var extendResult = (long)_redisCache.GetDatabase().ScriptEvaluate(ExtendIfMatchingValueScript, new RedisKey[] { Resource }, new RedisValue[] { LockId, (long)_expiryTime.TotalMilliseconds });

                result = extendResult == 1 ? LockInstanceResult.Success : extendResult == -1 ? LockInstanceResult.Conflicted : LockInstanceResult.Error;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error extending lock instance: {ex.Message}");
                result = LockInstanceResult.Error;
            }

            _logger.LogTrace($"Extend exit: {Resource}, {LockId}, {result}");

            return result;
        }

        private long GetRemainingValidityTicks(Stopwatch sw)
        {
            var driftTicks = (long)(_expiryTime.Ticks * _clockDriftFactor) + TimeSpan.FromMilliseconds(2).Ticks;
            var validityTicks = _expiryTime.Ticks - sw.Elapsed.Ticks - driftTicks;
            return validityTicks;
        }

        public override void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            _logger.LogDebug($"Disposing {Resource} ({LockId})");

            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                lock(_lockObject)
                {
                    if (_lockKeepaliveTimer != null)
                    {
                        _lockKeepaliveTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        _lockKeepaliveTimer.Dispose();
                        _lockKeepaliveTimer = null;
                    }
                }
            }

            UnLock();

            Status = LockStatus.Unlocked;
            _isDisposed = true;
        }
    }
}
