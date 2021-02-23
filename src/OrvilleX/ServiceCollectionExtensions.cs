using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using OrvilleX.Converts;
using OrvilleX.Dependency;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 标准基础库依赖
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 创建依赖注入对象并自动根据接口注入
        /// </summary>
        public static IDependencyBuilder CreateContainer(this IServiceCollection serviceCollection)
        {
            var builder = new DependencyBuilder(serviceCollection);
            return builder;
        }

        /// <summary>
        /// 提供Json序列化功能
        /// </summary>
        public static IServiceCollection AddJson(this IServiceCollection services)
        {
            services.AddSingleton<IJsonConvertProvider>(new JsonConvertProvider());
            return services;
        }

        /// <summary>
        /// 添加日志服务
        /// </summary>
        public static void UseLog(this IWebHostBuilder builder)
        {
            builder.UseNLog();
        }

        /// <summary>
        /// 配置日主配置文件路径
        /// </summary>
        /// <param name="configFileName">配置文件</param>
        public static ILoggerFactory ConfigureLog(this ILoggerFactory loggerFactory, string configFileName)
        {
            NLogBuilder.ConfigureNLog(configFileName);
            return loggerFactory;
        }
    }
}
