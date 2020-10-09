using Microsoft.Extensions.DependencyInjection;
using Sino.Extensions.Redis;
using Sino.Extensions.Redis.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sino.RedisTest
{
    public class RedisLockTest
    {
        private IDistributedLockProvider _client;

        public RedisLockTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddRedisCache(x =>
            {
                x.Host = "192.168.3.40";
            });
            services.AddRedisLock();

            _client = services.BuildServiceProvider().GetService<IDistributedLockProvider>();
        }

        [Fact]
        public async Task LockTest()
        {
            string lockkey = "lock1";

            var selflock = _client.CreateLock(lockkey, TimeSpan.FromSeconds(5));
            ILock otherlock = null;

            using (selflock)
            {
                if (selflock.IsAcquired)
                {
                    otherlock = _client.CreateLock(lockkey, TimeSpan.FromSeconds(5));
                    await Task.Delay(5100);
                }
            }

            using (otherlock)
            {
                if (otherlock.IsAcquired)
                {
                    Assert.True(false);
                }
            }
        }

        [Fact]
        public async Task LockTwoTest()
        {
            string lockkey = "lock2";

            var selflock = _client.CreateLock(lockkey, TimeSpan.FromSeconds(5));
            ILock otherlock = null;

            using(selflock)
            {
                if (selflock.IsAcquired)
                {
                    await Task.Delay(5100);
                }
            }
            otherlock = _client.CreateLock(lockkey, TimeSpan.FromSeconds(5));

            using (otherlock)
            {
                if (!otherlock.IsAcquired)
                {
                    Assert.True(false);
                }
            }
        }
    }
}
