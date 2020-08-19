using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
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
        /// 创建依赖注入对象
        /// </summary>
        public static IServiceProvider CreateContainer(this IServiceCollection serviceCollection)
        {
            var container = new WindsorContainer();
            container.Kernel.AddSubSystem(
                SubSystemConstants.NamingKey,
                new DependencyInjectionNamingSubsystem()
            );

            if (serviceCollection == null)
            {
                return container.Resolve<IServiceProvider>(); ;
            }

            container.Register(
                Component.For<IWindsorContainer>().Instance(container),
                Component.For<IServiceProvider, ISupportRequiredService>().ImplementedBy<SinoScopedServiceProvider>(),
                Component.For<IServiceScopeFactory>().ImplementedBy<SinoScopeFactory>().LifestyleSingleton());

            container.Kernel.Resolver.AddSubResolver(new RegisteredCollectionResolver(container.Kernel));
            container.Kernel.Resolver.AddSubResolver(new OptionsSubResolver(container.Kernel));
            container.Kernel.Resolver.AddSubResolver(new LoggerDependencyResolver(container.Kernel));

            foreach (var service in serviceCollection)
            {
                IRegistration registration;
                if (service.ServiceType.ContainsGenericParameters)
                {
                    registration = RegistrationAdapter.FromOpenGenericServiceDescriptor(service);
                }
                else
                {
                    var method = typeof(RegistrationAdapter).GetMethod("FromServiceDescriptor", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(service.ServiceType);
                    registration = method.Invoke(null, new object[] { service }) as IRegistration;
                }
                container.Register(registration);
            }

            return container.Resolve<IServiceProvider>(); ;
        }
    }
}
