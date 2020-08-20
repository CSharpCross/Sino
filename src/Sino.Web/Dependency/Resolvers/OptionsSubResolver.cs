using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Sino.Web.Dependency.Resolvers
{
    public class OptionsSubResolver : ISubDependencyResolver
    {
        private readonly IKernel _kernel;

        public OptionsSubResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            return dependency.TargetType != null &&
                dependency.TargetType.GetTypeInfo().IsGenericType &&
                dependency.TargetType.GetGenericTypeDefinition() == typeof(IOptions<>);
        }

        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            return _kernel.Resolve(dependency.TargetType);
        }
    }
}
