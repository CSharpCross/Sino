using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using OrvilleX.Dependency.Scope;
using System;

namespace OrvilleX.Dependency
{
    public class DefaultScopeFactory : IServiceScopeFactory
    {
        private readonly IWindsorContainer _container;

        public DefaultScopeFactory(IWindsorContainer container)
        {
            _container = container;
        }

        public IServiceScope CreateScope()
        {
            var scope = ExtensionContainerScope.BeginScope(ExtensionContainerScope.Current);
            var provider = _container.Resolve<IServiceProvider>();

            return new ServiceScope(scope, provider);
        }
    }
}
