using Microsoft.Extensions.Configuration;
using OrvilleX.AutoIndex;
using OrvilleX.AutoIndex.Generator;
using OrvilleX.AutoIndex.Mock;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddTinyId(this IServiceCollection services, IConfigurationSection section)
        {
            var cfg = new TinyIdClientConfiguration();
            section.Bind(cfg);
            services.AddSingleton(cfg);

            services.AddSingleton<ITinyIdClient, TinyIdClient>();

            if (cfg.Servers == null || cfg.Servers.Count == 0)
            {
                services.AddSingleton<IIdGeneratorFactory, MockIdGeneratorFactory>();
            } 
            else
            {

                services.AddSingleton<IIdGeneratorFactory, IdGeneratorFactoryClient>();
            }

            return services;
        }

        public static IServiceCollection AddTinyId(this IServiceCollection services, string token, params string[] servers)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            if (servers == null || servers.Length <= 0)
                throw new ArgumentNullException(nameof(servers));

            var cfg = new TinyIdClientConfiguration
            {
                Token = token,
                Servers = servers.ToList()
            };

            services.AddSingleton(cfg);
            services.AddSingleton<ITinyIdClient, TinyIdClient>();
            services.AddSingleton<IIdGeneratorFactory, IdGeneratorFactoryClient>();

            return services;
        }
    }
}
