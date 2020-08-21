using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sino.Web.Test
{
    public class DependencyBestConstructorTest
    {
        private IServiceCollection _serviceCollection;

        public DependencyBestConstructorTest()
        {
            _serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void ConstructorWithMoreArgumentsTest()
        {
            _serviceCollection.AddTransient<A>()
                .AddTransient<B>()
                .AddTransient<C>()
                .AddTransient<ServiceUser>();
            var container = _serviceCollection.CreateContainer().Build();

            var service = container.GetService<ServiceUser>();

            Assert.NotNull(service);
            Assert.NotNull(service.AComponent);
            Assert.NotNull(service.BComponent);
            Assert.NotNull(service.CComponent);
        }

        [Fact]
        public void ConstructorWithOneArgumentTest()
        {
            var container = _serviceCollection.AddTransient<A>()
                .AddTransient<ServiceUser>()
                .CreateContainer().Build();

            var service = container.GetService<ServiceUser>();

            Assert.NotNull(service);
            Assert.NotNull(service.AComponent);
            Assert.Null(service.BComponent);
            Assert.Null(service.CComponent);
        }

        [Fact]
        public void ConstructorWithTwoArgumentsTest()
        {
            var container = _serviceCollection.AddTransient<A>()
                .AddTransient<B>()
                .AddTransient<ServiceUser>()
                .CreateContainer().Build();

            var service = container.GetService<ServiceUser>();

            Assert.NotNull(service);
            Assert.NotNull(service.AComponent);
            Assert.NotNull(service.BComponent);
            Assert.Null(service.CComponent);
        }
    }

    public class A { }

    public class B
    {
        public B(A a)
        {
            A = a;
        }

        public A A { get; private set; }
    }

    public class C
    {
        public C(B b)
        {
            B = b;
        }

        public B B { get; private set; }
    }

    public class ServiceUser
    {
        public ServiceUser(A a)
		{
			if (a == null)
			{
				throw new ArgumentNullException();
			}
			AComponent = a;
		}

		public ServiceUser(A a, B b) : this(a)
		{
			if (b == null)
			{
				throw new ArgumentNullException();
			}
			BComponent = b;
		}

		public ServiceUser(A a, B b, C c) : this(a, b)
		{
			if (c == null)
			{
				throw new ArgumentNullException();
			}
			CComponent = c;
		}

        public A AComponent { get; }

        public B BComponent { get; }

        public C CComponent { get; }
    }
}
