using Microsoft.Extensions.DependencyInjection;
using OrvilleX.Cache;
using System.Threading.Tasks;
using Xunit;

namespace OrvilleXCacheTest
{
    public class KeyTest
    {
        private IRedisCache _client;

        public KeyTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddRedisCache(x =>
            {
                x.Host = "192.168.3.40";
            });

            _client = services.BuildServiceProvider().GetService<IRedisCache>();
        }

        [Fact]
        public async Task ExistsTest()
        {
            string key = "test1";
            _client.Remove(key);

            bool isExisted = _client.Exists(key);
            Assert.False(isExisted);

            isExisted = await _client.ExistsAsync(key);
            Assert.False(isExisted);

            _client.Set(key, "1");

            isExisted = _client.Exists(key);
            Assert.True(isExisted);

            isExisted = await _client.ExistsAsync(key);
            Assert.True(isExisted);
        }

        [Fact]
        public async Task RemoveTest()
        {
            string key = "test2";

            _client.Remove(key);
            await _client.RemoveAsync(key);

            _client.Set(key, "2");
            _client.Remove(key);

            Assert.False(_client.Exists(key));

            _client.Set(key, "3");
            await _client.RemoveAsync(key);

            Assert.False(_client.Exists(key));
        }

        [Fact]
        public async Task ExpireTest()
        {
            string key = "test3";
            _client.Remove(key);

            bool isset = _client.Expire(key, 30);
            Assert.False(isset);

            _client.Set(key, "3");

            isset = _client.Expire(key, 30);
            Assert.True(isset);

            isset = await _client.ExpireAsync(key, 35);
            Assert.True(isset);

            isset = _client.PExpire(key, 25000);
            Assert.True(isset);

            isset = await _client.PExpireAsync(key, 23000);
            Assert.True(isset);
        }
    }
}
