using OrvilleX.Dependency.Aop;

namespace OrvilleXTest.Fake
{
    [DefaultInterceptor("fooInterceptor")]
    public class CalculatorServiceWithFooInterceptorNamed : CalculatorService
    {
    }
}
