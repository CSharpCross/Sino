using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle.Scoped;
using System;
using System.Threading;

namespace OrvilleX.Dependency.Scope
{
    /// <summary>
    /// 扩展依赖解析上下文范围
    /// </summary>
    public class ExtensionContainerScope : ILifetimeScope, IDisposable
    {
        public static ExtensionContainerScope Current => current.Value;

        public static string TransientMarker = "Transient";

        protected static readonly AsyncLocal<ExtensionContainerScope> current = new AsyncLocal<ExtensionContainerScope>();

        private readonly ExtensionContainerScope parent;

        private readonly IScopeCache scopeCache;

        protected ExtensionContainerScope(ExtensionContainerScope parent)
        {
            scopeCache = new ScopeCache();
            if (parent == null)
            {
                this.parent = ExtensionContainerRootScope.RootScope;
            }
            else
            {
                this.parent = parent;
            }
        }

        public static ExtensionContainerScope BeginScope(ExtensionContainerScope parent)
        {
            var scope = new ExtensionContainerScope(parent);
            current.Value = scope;
            return scope;
        }

        public void Dispose()
        {
            var disposableCache = scopeCache as IDisposable;
            if (disposableCache != null)
            {
                disposableCache.Dispose();
            }

            current.Value = parent;
        }

        public Burden GetCachedInstance(ComponentModel model, ScopedInstanceActivationCallback createInstance)
        {
            if (model.Configuration.Attributes.Get(TransientMarker) == Boolean.TrueString)
            {
                var burden = createInstance((_) => { });
                scopeCache[burden] = burden;
                return burden;
            }
            else
            {
                var burden = scopeCache[model];
                if (burden == null)
                {
                    scopeCache[model] = burden = createInstance((_) => { });
                }

                return burden;
            }
        }

        internal class ForcedScope : IDisposable
        {
            private readonly ExtensionContainerScope previousScope;
            public ForcedScope(ExtensionContainerScope scope)
            {
                previousScope = ExtensionContainerScope.Current;
                ExtensionContainerScope.current.Value = scope;
            }
            public void Dispose()
            {
                ExtensionContainerScope.current.Value = previousScope;
            }
        }
    }
}
