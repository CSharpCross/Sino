using Microsoft.Extensions.DependencyInjection;
using Sino.Web.Dependency.Scope;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Web.Dependency
{
    public class SinoServiceProviderFactory : IServiceProviderFactory<IDependencyBuilder>
    {
		private readonly ExtensionContainerRootScope rootScope;

		public SinoServiceProviderFactory()
		{
			rootScope = ExtensionContainerRootScope.BeginRootScope();
		}

		public IDependencyBuilder CreateBuilder(IServiceCollection services)
		{
			var container = services.CreateContainer();
			return container;
		}

		public IServiceProvider CreateServiceProvider(IDependencyBuilder container)
		{
			return container.Build();
		}
	}
}
