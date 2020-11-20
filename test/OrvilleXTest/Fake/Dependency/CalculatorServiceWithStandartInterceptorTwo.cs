using OrvilleX.Dependency.Aop;

namespace OrvilleXTest.Fake
{
    [DefaultInterceptor("fooInterceptor")]
	[DefaultInterceptor(typeof(DefaultInterceptor))]
	public class CalculatorServiceWithStandartInterceptorTwo : CalculatorService
	{

	}
}
