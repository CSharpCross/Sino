using Castle.DynamicProxy;

namespace OrvilleX.Dependency.Aop
{
    /// <summary>
    /// AOP根类
    /// </summary>
    public class SinoInterceptor : ISinoInterceptor
    {
		public void Intercept(IInvocation invocation)
		{
			PreProceed(invocation);
			PerformProceed(invocation);
			PostProceed(invocation);
		}

		protected virtual void PerformProceed(IInvocation invocation)
		{
			invocation.Proceed();
		}

		protected virtual void PreProceed(IInvocation invocation)
		{
		}

		protected virtual void PostProceed(IInvocation invocation)
		{
		}
	}
}
