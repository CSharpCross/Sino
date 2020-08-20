using Castle.DynamicProxy;
using Sino.Web.Dependency.Aop;
using System;
using System.Collections.Generic;

namespace Sino.Web.Dependency
{
    public interface IDependencyBuilder
    {
        /// <summary>
        /// 添加需要自动注入的模块类型，多次调用将会叠加。
        /// </summary>
        /// <param name="types">类型列表</param>
        IDependencyBuilder AddAssembly(IEnumerable<Type> types);

        /// <summary>
        /// 添加AOP对象
        /// </summary>
        /// <typeparam name="TInterceptor">AOP类型</typeparam>
        /// <param name="name">AOP名称，可选</param>
        IDependencyBuilder AddInterceptor<TInterceptor>(string name = null) where TInterceptor : ISinoInterceptor;

        /// <summary>
        /// 构建依赖注入对象，需要将其进行返回。
        /// </summary>
        IServiceProvider Build();
    }
}
