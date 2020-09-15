# 依赖注入  

## 额外支持特性  

通过替换系统默认的IOC从而能够实现更丰富的功能，为此我们需要了解并掌握使用
这些新特性，下述文档将会通过相关代码实例介绍具体的使用方式。  

### 多构造函数  

在大多数实际使用中，我们往往会将需要注入的对象同构构造函数逐一列举，防止
依赖注入无法选择适合的构造函数，而在全新的依赖注入框架下我们就能够实现多
构造函数，并根据当前实际已存在的对象使IOC自行选择，从而实现更加丰富的功能
支持，下面将通过几个简单的例子进行说明。  


将设我们定以了`A`、`B`、`C`三个类，并且`ServiceUser`类存在以下三种构造函数。  

```csharp
public ServiceUser(A a)
public ServiceUser(A a, B b) : this(a)
public ServiceUser(A a, B b, C c) : this(a, b)
```

此时当我们将`ServiceUser`通过IOC进行实例话的时候，其IOC会根据当前是否注册了
对应的`A`、`B`、`C`类来决定采用那个构造函数，意思就是当前如果仅有`A`类注册
了，那么就是调用`public ServiceUser(A a)`构造函数，如果存在`A`、`B`类则采用
`ServiceUser(A a, B b)`该构造函数，当然如果仅仅只有`B`类，当然就不满足任何
构造函数了。  

### 横截面切入  

随着业务的不断复杂化，很多场景都需要对方法进行一个全局的处理，为此我们很大程度上需要依赖AOP技术来
满足我们的需求，为此该库当前集成了其他库的AOP方法并进行了一定的缩减。以此来满足实际业务的需求，下
面我们将介绍如何使用这一特性。  

首先我们需要编写对应的`Interceptor`方法，这里需要继承`SinoInterceptor`类：  

```csharp
	public class CalculatorInterceptor : SinoInterceptor
	{
	    protected override void PerformProceed(IInvocation invocation)
		{
		   // 调用中
			invocation.Proceed();
		}

		protected override void PreProceed(IInvocation invocation)
		{
		   // 调用前
		}

		protected override void PostProceed(IInvocation invocation)
		{
		   // 调用结束后
		}
	}
```  

如上所示，其中可供我们重写的有三类方法，每个方法都对应了目标方法的不同生命周期。读者可以根据实际
的需要重写对应的方法，如果需要获取被调方法相关的信息则可以通过`invocation`对象来获取。最后我们需
要在`Startup`的`ConfigureServices`方法中将对应AOP的方法进行注入：  

```csharp
public IServiceProvider ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    var builder = services.CreateContainer();
	builder.AddInterceptor<SinoInterceptor>().AddInterceptor<SinoInterceptor>("FooInterceptor");
    return builder.Build();
}
```  

这里可以注意到注入的方式存在两种，即单纯的类型注入和带命名的注入。这就需要说到如何决定具体方法
了，这里需要使用`SinoInterceptorAttribute`注解属性来实现切入，将该注解属性写到对应的方法上并采
用类型或者名称的方式进行关联，如以下方式：  

```csharp
[SinoInterceptor("fooInterceptor")]
[SinoInterceptor(typeof(SinoInterceptor))]
public class CalculatorServiceWithStandartInterceptorTwo : CalculatorService
{

}
```  

完成后当方法经过IOC被调用方调用后我们的AOP就可以起作用了。  

