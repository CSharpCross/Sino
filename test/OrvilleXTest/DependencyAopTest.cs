using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using OrvilleX.Dependency.Aop;
using OrvilleXTest.Fake;
using Xunit;

namespace OrvilleXTest
{
    public class DependencyAopTest
    {
        private readonly IServiceCollection _serviceCollection;

        public DependencyAopTest()
        {
            _serviceCollection = new ServiceCollection();
        }

        private bool IsProxy(object instance)
        {
            return instance is IProxyTargetAccessor;
        }

        private Castle.DynamicProxy.IInterceptor[] GetInterceptors(object proxy)
        {
            return ((IProxyTargetAccessor)proxy).GetInterceptors();
        }

        [Fact]
        public void CanSetInterceptorWithNameTest()
        {
            _serviceCollection.AddTransient<ICalcService, CalculatorServiceWithFooInterceptorNamed>();
            var container = _serviceCollection.CreateContainer().AddInterceptor<DefaultInterceptor>("fooInterceptor").Build();

            var calcService = container.GetService<ICalcService>();

            Assert.True(IsProxy(calcService));
        }

        [Fact]
        public void CanSetInterceptorWithType()
        {
            _serviceCollection.AddTransient<ICalcService, CalculatorServiceWithStandartInterceptorTyped>();
            var container = _serviceCollection.CreateContainer().AddInterceptor<DefaultInterceptor>().Build();

            var calcService = container.GetService<ICalcService>();

            Assert.True(IsProxy(calcService));
        }

        [Fact]
        public void CanSetInterceptorWithMany()
        {
            _serviceCollection.AddTransient<ICalcService, CalculatorServiceWithStandartInterceptorTwo>();
            var container = _serviceCollection.CreateContainer().AddInterceptor<DefaultInterceptor>().AddInterceptor<DefaultInterceptor>("FooInterceptor").Build();

            var calcService = container.GetService<ICalcService>();

            Assert.True(IsProxy(calcService));
            Assert.Equal(2, GetInterceptors(calcService).Length);
        }
    }
}
