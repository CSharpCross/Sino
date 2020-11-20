using Microsoft.Extensions.DependencyInjection;
using OrvilleX.AutoIndex;
using System.Threading.Tasks;
using Xunit;

namespace OrvilleXAutoIndexTest
{
    public class TinyIdClientTest
    {
        private ITinyIdClient _client;

        public TinyIdClientTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTinyId("0f673adf80504e2eaa552f5d791b644c", "localhost");

            _client = services.BuildServiceProvider().GetService<ITinyIdClient>();
        }

        [Fact]
        public async Task GetNextIdTest()
        {
            long id = await _client.NextId("test");

            id = await _client.NextId("test_odd");

            Assert.True(id > 0);
        }

        [Fact]
        public async Task GetNextIdsTest()
        {
            var ids = await _client.NextId("test", 10);

            Assert.True(ids.Count == 10);

            ids = await _client.NextId("test_odd", 10);

            Assert.True(ids.Count == 10);
        }

        [Fact]
        public async Task GetNextIdFireLoadingTest()
        {
            long id = 0;
            for(var i = 0; i < 10; i++)
            {
                id = await _client.NextId("test");
            }

            long lid = await _client.NextId("test");

            Assert.True(lid > id);
        }
    }
}
