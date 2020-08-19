using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using Sino.Web.Dependency.Scope;
using System;

namespace Sino.Web.Dependency
{
    public class SinoScopeFactory : IServiceScopeFactory
    {
        private readonly IWindsorContainer _container;

        public SinoScopeFactory(IWindsorContainer container)
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
