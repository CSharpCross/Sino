using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Sino.Web.Dependency.Aop;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sino.Web.Test
{
    public class DependencyAopTest
    {
        private IServiceCollection _serviceCollection;

        public DependencyAopTest()
        {
            _serviceCollection = new ServiceCollection();
        }

        private bool IsProxy(object instance)
        {
            return instance is IProxyTargetAccessor;
        }

        private IInterceptor[] GetInterceptors(object proxy)
        {
            return ((IProxyTargetAccessor)proxy).GetInterceptors();
        }

        [Fact]
        public void CanSetInterceptorWithNameTest()
        {
            _serviceCollection.AddTransient<ICalcService, CalculatorServiceWithFooInterceptorNamed>();
            var container = _serviceCollection.CreateContainer().AddInterceptor<SinoInterceptor>("fooInterceptor").Build();

            var calcService = container.GetService<ICalcService>();

            Assert.True(IsProxy(calcService));
        }

        [Fact]
        public void CanSetInterceptorWithType()
        {
            _serviceCollection.AddTransient<ICalcService, CalculatorServiceWithStandartInterceptorTyped>();
            var container = _serviceCollection.CreateContainer().AddInterceptor<SinoInterceptor>().Build();

            var calcService = container.GetService<ICalcService>();

            Assert.True(IsProxy(calcService));
        }

        [Fact]
        public void CanSetInterceptorWithMany()
        {
            _serviceCollection.AddTransient<ICalcService, CalculatorServiceWithStandartInterceptorTwo>();
            var container = _serviceCollection.CreateContainer().AddInterceptor<SinoInterceptor>().AddInterceptor<SinoInterceptor>("FooInterceptor").Build();

            var calcService = container.GetService<ICalcService>();

            Assert.True(IsProxy(calcService));
            Assert.Equal(2, GetInterceptors(calcService).Length);
        }
    }
}
