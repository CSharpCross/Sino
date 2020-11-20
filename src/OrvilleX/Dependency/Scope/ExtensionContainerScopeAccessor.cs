using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle.Scoped;
using System;

namespace OrvilleX.Dependency.Scope
{
    public class ExtensionContainerScopeAccessor : IScopeAccessor
    {
        public ILifetimeScope GetScope(CreationContext context)
        {
            if (ExtensionContainerScope.Current == null)
            {
                throw new InvalidOperationException("No scope available");
            }
            return ExtensionContainerScope.Current;
        }

        public void Dispose() { }
    }
}
