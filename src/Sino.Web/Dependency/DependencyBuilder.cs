using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Sino.Web.Dependency.Aop;
using Castle.Windsor;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Sino.Web.Dependency.Resolvers;
using Sino.Dependency;
using System.Reflection;
using Sino.Web.Dependency.Scope;
using Sino.Web.Module;

namespace Sino.Web.Dependency
{
    /// <summary>
    /// 依赖注入生成对象
    /// </summary>
    public class DependencyBuilder : IDependencyBuilder
    {
        private readonly ExtensionContainerRootScope _rootScope;

        private IServiceCollection _serviceCollection;

        private List<Type> _types = new List<Type>();
        private List<InterceptorInfo> _interceptorInfo = new List<InterceptorInfo>();
        private IWindsorContainer _container;

        public DependencyBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _rootScope = ExtensionContainerRootScope.BeginRootScope();
            Init();
        }

        private void Init()
        {
            _container = new WindsorContainer();
            _container.Kernel.AddSubSystem(
                SubSystemConstants.NamingKey,
                new DependencyInjectionNamingSubsystem()
            );
        }

        public IDependencyBuilder AddAssembly(IEnumerable<Type> types)
        {
            if (types == null || types.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(types));
            }

            _types.AddRange(types);
            return this;
        }

        public IDependencyBuilder AddInterceptor<TInterceptor>(string name = null) where TInterceptor : ISinoInterceptor
        {
            _interceptorInfo.Add(new InterceptorInfo
            {
                InterceptorType = typeof(TInterceptor),
                Name = name
            });
            return this;
        }

        public IServiceProvider Build()
        {
            _container.Register(
                Component.For<IWindsorContainer>().Instance(_container),
                Component.For<IServiceProvider, ISupportRequiredService>().ImplementedBy<SinoScopedServiceProvider>().LifeStyle.Scoped<ExtensionContainerScopeAccessor>(),
                Component.For<IServiceScopeFactory>().ImplementedBy<SinoScopeFactory>().LifestyleSingleton());

            _container.Kernel.Resolver.AddSubResolver(new RegisteredCollectionResolver(_container.Kernel));
            _container.Kernel.Resolver.AddSubResolver(new OptionsSubResolver(_container.Kernel));
            _container.Kernel.Resolver.AddSubResolver(new LoggerDependencyResolver(_container.Kernel));

            foreach (var service in _serviceCollection)
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
                _container.Register(registration);
            }

            foreach(var aop in _interceptorInfo)
            {
                if (string.IsNullOrEmpty(aop.Name))
                {
                    _container.Register(
                        Component.For(aop.InterceptorType));
                }
                else
                {
                    _container.Register(
                        Component.For(aop.InterceptorType).Named(aop.Name));
                }
            }

            DependencyAutoRegister();

            var services = _container.Resolve<IServiceProvider>();

            ModuleAutoRegister(services);

            return services;
        }

        /// <summary>
        /// 基础依赖自动注入
        /// </summary>
        private void DependencyAutoRegister()
        {
            if (_types.Count > 0)
            {
                _container.Register(Classes.From(_types)
                    .BasedOn<ISingletonDependency>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .WithService.Self()
                    .WithService.DefaultInterfaces()
                    .LifestyleSingleton());

                _container.Register(Classes.From(_types)
                    .BasedOn<ITransientDependency>()
                    .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                    .WithService.Self()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());
            }
        }

        /// <summary>
        /// 模块依赖注入
        /// </summary>
        private void ModuleAutoRegister(IServiceProvider services)
        {
            var modules = services.GetServices<IModuleRegister>();
            foreach(var module in modules)
            {
                module.ConfigureServices(_container, _types);
            }
        }
    }
}
