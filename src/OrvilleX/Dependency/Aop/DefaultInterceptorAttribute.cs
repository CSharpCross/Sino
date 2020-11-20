using Castle.Core;
using System;

namespace OrvilleX.Dependency.Aop
{
    /// <summary>
    /// AOP注解属性
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DefaultInterceptorAttribute : InterceptorAttribute
    {
        /// <summary>
        /// 通过组件名
        /// </summary>
        public DefaultInterceptorAttribute(string componentKey)
            : base(componentKey) { }

        /// <summary>
        /// 通过组件类型
        /// </summary>
        public DefaultInterceptorAttribute(Type interceptorType)
            : base(interceptorType) { }
    }
}
