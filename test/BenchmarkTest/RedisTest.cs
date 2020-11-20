using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace BenchmarkTest
{
    [SimpleJob(RunStrategy.Throughput, RuntimeMoniker.NetCoreApp20)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class RedisTest
    {
        private IDatabase _database;

        public RedisTest()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.3.40:6379");
            _database = redis.GetDatabase();
        }

        [Benchmark]
        public async Task StringGetAndSetTest()
        {
            Random r = new Random();
            var key = r.Next(1000);
            await _database.StringSetAsync(key.ToString(), "testfefefaf");

            string value = await _database.StringGetAsync(key.ToString());
        }
    }
}
