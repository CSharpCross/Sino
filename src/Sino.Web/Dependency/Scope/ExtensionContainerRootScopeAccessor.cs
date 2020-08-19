using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle.Scoped;
using System;

namespace Sino.Web.Dependency.Scope
{
    public class ExtensionContainerRootScopeAccessor : IScopeAccessor
    {
        public ILifetimeScope GetScope(CreationContext context)
        {
            if (ExtensionContainerRootScope.RootScope == null)
            {
                throw new InvalidOperationException("No root scope");
            }
            return ExtensionContainerRootScope.RootScope;
        }

        public void Dispose() { }
    }
}
