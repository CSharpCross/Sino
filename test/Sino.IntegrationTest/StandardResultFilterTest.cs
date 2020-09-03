using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Sino.IntegrationTest.Fake;
using Sino.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Sino.IntegrationTest
{
    public class StandardResultFilterTest : IClassFixture<WebAppFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public StandardResultFilterTest(ITestOutputHelper output, WebAppFixture webApp)
        {
            _output = output;
            _client = webApp.WithWebHostBuilder(webHostBuilder =>
            {
                webHostBuilder.UseSolutionRelativeContentRoot("./");
                webHostBuilder.ConfigureServices(services =>
                {
                    services.AddStandardResultFilter();
                    services.AddFluentValidationForTesting(fv => { }, mvc =>
                    {
                        mvc.Filters.UseStandardResultFilter();
                    });
                });
            }).CreateClient();
        }

        [Fact]
        public async Task StandardResultFilterWithNormalTest()
        {
            var result = await _client.GetBody<BaseResponse<ParentModel>>("StandardResultFilterWithNormal", new Dictionary<string, string>());

            Assert.NotNull(result);
            Assert.Equal("0", result.ErrorCode);
            Assert.True(result.Success);
            Assert.Equal("test", result.Data.Child.Name);
        }

        [Fact]
        public async Task StandardResultFilterWithNoUseTest()
        {
            var result = await _client.GetBody<ParentModel>("StandardResultFilterWithNoUse", new Dictionary<string, string>());

            Assert.NotNull(result);
            Assert.Equal("nouse", result.Child.Name);
        }
    }
}
