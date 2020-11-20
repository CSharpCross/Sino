using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Microsoft.Extensions.Logging;

namespace OrvilleX.Dependency.Resolvers
{
    public class LoggerDependencyResolver : ISubDependencyResolver
    {
        private readonly IKernel _kernel;

        public LoggerDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            return dependency.TargetType == typeof(ILogger);
        }

        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            var factory = _kernel.Resolve<ILoggerFactory>();
            return factory.CreateLogger(RegistrationAdapter.OriginalComponentName(model.Name));
        }
    }
}
