using Microsoft.AspNetCore.TestHost;
using Sino.ViewModels;
using Sino.Web.Validation;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Sino.IntegrationTest
{
    public class ValidationTest : IClassFixture<WebAppFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public ValidationTest(ITestOutputHelper output, WebAppFixture webApp)
        {
            _output = output;
            _client = webApp.WithWebHostBuilder(webHostBuilder =>
            {
                webHostBuilder.UseSolutionRelativeContentRoot("./");
                webHostBuilder.ConfigureServices(services =>
                {
                    services.AddFluentValidationForTesting(fv =>
                    {
                        fv.ImplicitylyValidateChildProperties = false;
                        fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                    }, mvc => { });
                });
            }).CreateClient();
        }

        [Fact]
        public async Task ResolvesExplicitChildValidatorTest()
        {
            try
            {
                var result = await _client.GetBody<BaseResponse>("InjectsExplicitChildValidator", new Dictionary<string, string>());
            }
            catch (SinoException ex)
            {
                Assert.Equal(BaseRequestValidator<object>.DEFAULT_ERROR_CODE, ex.Code);
                Assert.Equal("NotNullInjected", ex.Message);
            }
        }

        [Fact]
        public async Task ResolvesExplicitChildValidatorForCollection()
        {
            try
            {
                var formData = new Dictionary<string, string>();
                formData.Add("Children[0].Name", null);
                var result = await _client.GetBody<BaseResponse>("InjectsExplicitChildValidatorCollection", formData);
            }
            catch (SinoException ex)
            {
                Assert.Equal(BaseRequestValidator<object>.DEFAULT_ERROR_CODE, ex.Code);
                Assert.Equal("NotNullInjected", ex.Message);
            }
        }
    }
}
