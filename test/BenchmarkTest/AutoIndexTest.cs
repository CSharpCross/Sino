using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.DependencyInjection;
using OrvilleX.AutoIndex;
using System.Threading.Tasks;

namespace BenchmarkTest
{
    [SimpleJob(RunStrategy.Throughput, RuntimeMoniker.NetCoreApp20)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class AutoIndexTest
    {
        private ITinyIdClient _client;

        public AutoIndexTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTinyId("0f673adf80504e2eaa552f5d791b644c", "localhost");

            _client = services.BuildServiceProvider().GetService<ITinyIdClient>();
        }

        [Benchmark]
        public async Task SingleNextId()
        {
            _ = await _client.NextId("test");
        }

        [Benchmark]
        public async Task BatchNextId()
        {
            _ = await _client.NextId("test", 1000);
        }
    }
}
