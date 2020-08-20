using System;

namespace Sino.Web.Dependency.Aop
{
    /// <summary>
    /// AOP对象信息
    /// </summary>
    public class InterceptorInfo
    {
        public Type InterceptorType { get; set; }

        public string Name { get; set; }
    }
}
