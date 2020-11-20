using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace OrvilleXIntegrationTest
{
    public class WebAppFixture : WebApplicationFactory<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseDefaultServiceProvider((context, options) => options.ValidateScopes = true)
                .UseStartup<Startup>();
        }
    }
}
