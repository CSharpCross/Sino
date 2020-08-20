using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Sino.Dependency;
using Sino.Web;
using Sino.Web.Dependency;
using Sino.Web.Dependency.Resolvers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public static class DependencyServiceCollectionExtensions
    {
        /// <summary>
        /// 创建依赖注入对象并自动根据接口注入
        /// </summary>
        public static IDependencyBuilder CreateContainer(this IServiceCollection serviceCollection)
        {
            var builder = new DependencyBuilder(serviceCollection);
            return builder;
        }
    }
}
