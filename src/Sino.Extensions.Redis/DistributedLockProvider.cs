using Microsoft.Extensions.Logging;
using Sino.Extensions.Redis.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sino.Extensions.Redis
{
    public class DistributedLockProvider : IDistributedLockProvider
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IRedisCache _redisCache;

        public DistributedLockProvider(IRedisCache redisCache, ILoggerFactory loggerFactory)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public ILock CreateLock(string resource, TimeSpan expiryTime)
        {
            return RedisLock.Create(_loggerFactory.CreateLogger<RedisLock>(),
                _redisCache,
                resource,
                expiryTime);
        }

        public ILock CreateLock(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            return RedisLock.Create(_loggerFactory.CreateLogger<RedisLock>(),
                _redisCache,
                resource,
                expiryTime,
                waitTime,
                retryTime,
                cancellationToken);
        }

        public async Task<ILock> CreateLockAsync(string resource, TimeSpan expiryTime)
        {
            return await RedisLock.CreateAsync(_loggerFactory.CreateLogger<RedisLock>(),
                _redisCache,
                resource,
                expiryTime).ConfigureAwait(false);
        }

        public async Task<ILock> CreateLockAsync(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            return await RedisLock.CreateAsync(_loggerFactory.CreateLogger<RedisLock>(),
                _redisCache,
                resource,
                expiryTime,
                waitTime,
                retryTime,
                cancellationToken).ConfigureAwait(false);
        }
    }
}
