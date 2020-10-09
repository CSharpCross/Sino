using Microsoft.Extensions.Configuration;
using Sino.Extensions.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RedisCacheServiceCollectionExtensions
    {
        /// <summary>
        /// 增加Redis扩展
        /// </summary>
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfigurationSection section)
        {
            var mainCfg = new RedisCacheOptions();
            section.Bind(mainCfg);
            services.AddSingleton(mainCfg);

            services.AddOptions();
            services.Add(ServiceDescriptor.Singleton<IRedisCache, RedisCache>());

            return services;
        }

        /// <summary>
        /// 增加Redis扩展
        /// </summary>
        /// <param name="setupAction">配置</param>
        public static IServiceCollection AddRedisCache(this IServiceCollection services, Action<RedisCacheOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();
            services.Configure(setupAction);
            services.Add(ServiceDescriptor.Singleton<IRedisCache, RedisCache>());

            return services;
        }

        /// <summary>
        /// 增加基于Redis的分布式锁
        /// </summary>
        public static IServiceCollection AddRedisLock(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(ServiceDescriptor.Singleton<IDistributedLockProvider, DistributedLockProvider>());

            return services;
        }
    }
}
