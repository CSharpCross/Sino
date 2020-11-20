using Microsoft.Extensions.DependencyInjection;
using System;

namespace OrvilleX.Dependency
{
    public class ServiceScope : IServiceScope
    {
        private readonly IDisposable _scope;

        private readonly IServiceProvider _serviceProvider;

        public ServiceScope(IDisposable sinoScope, IServiceProvider serviceProvider)
        {
            _scope = sinoScope ?? throw new ArgumentNullException(nameof(sinoScope));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IServiceProvider ServiceProvider => _serviceProvider;

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
