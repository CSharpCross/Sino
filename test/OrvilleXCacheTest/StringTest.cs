using Microsoft.Extensions.DependencyInjection;
using OrvilleX.Cache;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OrvilleXCacheTest
{
    public class StringTest
    {
        private IRedisCache _client;

        public StringTest()
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
        public async Task AppendTest()
        {
            string key = "string1";
            _client.Remove(key);

            long size = _client.Append(key, "1");
            size = await _client.AppendAsync(key, "234");

            Assert.Equal(4, size);
        }

        [Fact]
        public async Task BitCountTest()
        {
            string key = "string2";
            _client.Remove(key);

            _client.Set(key, "11011");

            long count = _client.BitCount(key);
            Assert.Equal(14, count);

            count = await _client.BitCountAsync(key);
            Assert.Equal(14, count);
        }

        [Fact]
        public async Task DecrAndIncrTest()
        {
            string key = "string3";
            _client.Remove(key);

            long num = _client.Incr(key);
            Assert.Equal(1, num);

            num = await _client.IncrAsync(key);
            Assert.Equal(2, num);

            num = _client.IncrBy(key, 20);
            Assert.Equal(22, num);

            num = await _client.IncrByAsync(key, 2);
            Assert.Equal(24, num);

            num = _client.Decr(key);
            Assert.Equal(23, num);

            num = await _client.DecrAsync(key);
            Assert.Equal(22, num);

            num = _client.DecrBy(key, 10);
            Assert.Equal(12, num);

            num = await _client.DecrByAsync(key, 2);
            Assert.Equal(10, num);
        }

        [Fact]
        public async Task SetAndGetBitTest()
        {
            string key = "string4";
            _client.Remove(key);

            _client.SetBit(key, 1, true);
            _client.SetBit(key, 20, true);
            await _client.SetBitAsync(key, 21, true);

            bool bit = _client.GetBit(key, 1);
            Assert.True(bit);

            bit = _client.GetBit(key, 15);
            Assert.False(bit);

            bit = await _client.GetBitAsync(key, 20);
            Assert.True(bit);

            bit = await _client.GetBitAsync(key, 21);
            Assert.True(bit);
        }

        [Fact]
        public async Task RangeTest()
        {
            string key = "string5";
            _client.Remove(key);

            _client.Set(key, "1234556", 30);

            string val = _client.GetRange(key, 0, 3);
            Assert.Equal("1234", val);

            val = await _client.GetRangeAsync(key, 3, 5);
            Assert.Equal("455", val);

            long len = _client.StrLen(key);
            long len1 = await _client.StrLenAsync(key);
            Assert.Equal(len, len1);
        }

        [Fact]
        public async Task SetNxTest()
        {
            string key = "string6";
            _client.Remove(key);

            bool ok = _client.SetNx(key, "123", TimeSpan.FromSeconds(10));
            Assert.True(ok);

            ok = await _client.SetNxAsync(key, "456", TimeSpan.FromSeconds(5));
            Assert.False(ok);
        }
    }
}
