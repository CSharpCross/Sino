using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using Sino.Web.Dependency.Scope;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sino.Web.Dependency
{
    /// <summary>
    /// 提供依赖注入服务
    /// </summary>
    internal class SinoScopedServiceProvider : IServiceProvider, ISupportRequiredService, IDisposable
    {
        private readonly ExtensionContainerScope _scope;

        private bool _disposing = false;

        private readonly IWindsorContainer _container;

        public SinoScopedServiceProvider(IWindsorContainer container)
        {
            _container = container;
            _scope = ExtensionContainerScope.Current;
        }

        public object GetService(Type serviceType)
        {
            using(var fs = new ExtensionContainerScope.ForcedScope(_scope))
            {
                return ResolveInstanceOrNull(serviceType, true);
            }
        }

        public object GetRequiredService(Type serviceType)
        {
            using(var fs = new ExtensionContainerScope.ForcedScope(_scope))
            {
                return ResolveInstanceOrNull(serviceType, false);
            }
        }

        public void Dispose()
        {
            if (_scope is ExtensionContainerRootScope)
            {
                if (!_disposing)
                {
                    _disposing = true;
                    var disposableScope = _scope as IDisposable;
                    if (disposableScope != null)
                    {
                        disposableScope.Dispose();
                    }
                    _container.Dispose();
                }
            }
        }

        private object ResolveInstanceOrNull(Type serviceType, bool isOptional)
        {
            if (_container.Kernel.HasComponent(serviceType))
            {
                return _container.Resolve(serviceType);
            }

            if (serviceType.GetTypeInfo().IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var allObjects = _container.ResolveAll(serviceType.GenericTypeArguments[0]);
                return allObjects;
            }

            if (isOptional)
            {
                return null;
            }

            return _container.Resolve(serviceType);
        }
    }
}
