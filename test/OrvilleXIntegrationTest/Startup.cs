using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrvilleX.Validation;
using System;

namespace OrvilleXIntegrationTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public static class WebTestExtensions
    {
        public static void AddFluentValidationForTesting(this IServiceCollection services, Action<ValidationMvcConfiguration> configurator, Action<MvcOptions> setupAction)
        {
			var mvcBuilder = services.AddMvc(setupAction);

            mvcBuilder.AddValidation(configurator);
        }
    }
}
