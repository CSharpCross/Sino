using Microsoft.Extensions.Logging;
using Sino.Extensions.Redis.Internal;
using Sino.Extensions.Redis.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sino.Extensions.Redis
{
    /// <summary>
    /// 基于Redis的分布式锁
    /// </summary>
    public class RedisLock : AbstractLock
    {
        private readonly object _lockObject = new object();

        private readonly IRedisCache _redisCache;
        private readonly ILogger<RedisLock> _logger;

        private readonly int _quorum;
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

            _quorum = 1;
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
                    (Status, InstanceSummary) = Acquire();
                }

                if (!IsAcquired)
                {
                    
                }
            }
        }

        private async Task StartAsync()
        {

        }

        private (LockStatus, LockInstanceSummary) Acquire()
        {

        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
