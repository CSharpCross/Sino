using OrvilleX.Dependency.Aop;

namespace OrvilleXTest.Fake
{
    [DefaultInterceptor(typeof(DefaultInterceptor))]
	public class CalculatorServiceWithStandartInterceptorTyped : CalculatorService
	{

	}
}
