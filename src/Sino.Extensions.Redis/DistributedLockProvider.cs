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
            throw new NotImplementedException();
        }

        public ILock CreateLock(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<ILock> CreateLockAsync(string resource, TimeSpan expiryTime)
        {
            throw new NotImplementedException();
        }

        public Task<ILock> CreateLockAsync(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
    }
}
