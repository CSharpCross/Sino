using Castle.MicroKernel.Lifestyle.Scoped;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Sino.Web.Dependency.Scoped
{
    /// <summary>
    /// 扩展依赖解析上下文范围
    /// </summary>
    public class ExtensionContainerScope : ILifetimeScope, IDisposable
    {
        public static ExtensionContainerScope Current => current.Value;

        protected static readonly AsyncLocal<ExtensionContainerScope> current = new AsyncLocal<ExtensionContainerScope>();
    }
}
