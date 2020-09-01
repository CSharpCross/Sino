using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.IntegrationTest
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
