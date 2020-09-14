using Microsoft.Extensions.Configuration;
using Sino.Extensions.AutoIndex;
using Sino.Extensions.AutoIndex.Generator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoIndexServiceCollectionExtensions
    {
        public static IServiceCollection AddTinyId(this IServiceCollection services, IConfigurationSection section)
        {
            var cfg = new TinyIdClientConfiguration();
            section.Bind(cfg);
            services.AddSingleton(cfg);

            services.AddSingleton<ITinyIdClient, TinyIdClient>();
            services.AddSingleton<IIdGeneratorFactory, IdGeneratorFactoryClient>();

            return services;
        }
    }
}
