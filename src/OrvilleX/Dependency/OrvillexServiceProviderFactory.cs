using OrvilleX.Dependency;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class OrvillexServiceProviderFactory : IServiceProviderFactory<IDependencyBuilder>
    {
        private readonly Action<IDependencyBuilder> _configurationAction;

        public OrvillexServiceProviderFactory(Action<IDependencyBuilder> configurationAction = null) =>
            _configurationAction = configurationAction ?? (builder => { });

        public IDependencyBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new DependencyBuilder(services);

            _configurationAction(builder);

            return builder;
        }

        public IServiceProvider CreateServiceProvider(IDependencyBuilder containerBuilder)
        {
            if (containerBuilder == null) throw new ArgumentNullException(nameof(containerBuilder));

            return containerBuilder.Build();
        }
    }
}
