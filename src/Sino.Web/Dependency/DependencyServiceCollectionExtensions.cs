using Castle.MicroKernel;
using Castle.Windsor;
using Sino.Web.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public static class DependencyServiceCollectionExtensions
    {
        /// <summary>
        /// 创建依赖注入对象
        /// </summary>
        public static IDependencyContainer CreateContainer(this ServiceCollection serviceCollection)
        {
            var container = new WindsorContainer();
            container.Kernel.AddSubSystem(
                SubSystemConstants.NamingKey,
                new 
                );
        }
    }
}
