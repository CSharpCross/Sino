# Sino.Web
该项目用于规范后期所有后台开发的规范并为后期更好的升级做好前期准备工作。   

[![Build status](https://ci.appveyor.com/api/projects/status/b64xdqtjcmj8syo9/branch/dev?svg=true)](https://ci.appveyor.com/project/vip56/sino-web/branch/dev)
[![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg?style=plastic)](https://www.nuget.org/packages/Sino.Web)   

## 如何使用
```
Install-Package Sino.Web
```

# 领域开发标准
下面将介绍如何使用DDD进行业务系统的开发。  

## 领域层

### 领域模型
按照规定所有的领域模型必须继承自`FullAuditedEntity<TPrimaryKey>`虚类，其中`TPrimaryKey`是用于规定主键的类型，为了能够和仓储
层自动配合建议使用`Guid`以及`Int`类型，为什么需要继承该模型，因为该模型中集成了我们正常开发中需要使用的常用字段，
具体如下所示：  
* `IsDeleted（bool）`：提供软删除功能，默认为False。
* `DeletionTime（DateTime?）`：删除的时间。
* `DeleterUserId（long?）`：执行删除操作的用户。
* `LastModificationTime（DateTime）`：最后编辑时间，所有的UPDATE操作。
* `LastModifierUserId（long?）`：最后编辑的用户，该该值赋值的同时`LastModificationTime`自动赋为当前时间。
* `CreationTime（DateTime）`：创建时间，创建模型的时候自动赋值为当前时间。
* `CreatorUserId（long?）`：创建的用户
* `Id`：根据`TPrimaryKey`类型所定

除了以上这些扩展的字段之外，还提供了其他的快捷方法：
* `IsNullOrDeleteed():bool`：如果当前实体为Null或IsDeleted为True则返回True，否则为False。
* `UnDelete`：撤销删除操作，即将`IsDeleted`设置为False，同时将`DeletionTime`和`DeleterUserId`设置为Null。

还需要注意其中部分自带的方法已经被重写从而符合真实的业务场景：
* `Equals`：判断依据为`Id`字段是否相等。
* `GetHashCode`：获取的哈希值为`Id`字段。
* `==`：判断依据为`Id`字段。
* `!=`：判断依据为`Id`字段。
* `ToString`：输出为`[GetType().Nmae] [Id]`。

如果实际的业务并不需要依赖如此多的审计功能，可以单独选择继承其他类：
* `AuditedEntity`：不包含删除审计功能。
* `CreationAuditedEntity`：只包含创建信息。
* `Entity`：不包含任何审计功能。

### 领域服务
因为当前领域服务存在数量不多，所以只约定了需要统一继承的公共类为`DomainService`。

------

## 仓储层

### 仓储接口
准确的说仓储层所定义的接口在项目结构上是保存在领域层的项目中，以此规范仓储层的实现。具体需要继承的仓储接口需要根据根据
对应的实体模型的情况而决定，如果实体模型的主键类型为`Int`则对应的仓储接口则需要继承自`IRepository<TEntity>`，如果不是
该类型则需要继承自`IRepository<TEntity, TPrimaryKey>`，他们都是默认有很多常用的方法，当然这些方案并不需要我们自行实现。  

可以发现我们接口中的`Task<Tuple<int, IList<TEntity>>> GetListAsync(IQueryObject<TEntity> query);`中有一个`IQueryObject`
这是为了能够以最小的成本来增加新的查询条件，这样我们不用在仓储中看到很多查询条件的语句了，真实编码中我们需要继承自`QueryObject<Entity>`
类，其中`QueryExpression`提供了具体查询语句的Lambda表达式，而为了防止SQL注入，我们来利用了`QuerySql`属性来进行参数的传递。
当然除了查询条件，列表中经常使用的还有字段的排序，这里我们也提供了`OrderField`字段用来记录，从而保证我们的列表查询是统一的，
就算一后期有变动也只需要修改对应的`QueryObject`对象即可。  
PS：这里我们也考虑过使用动态的方式，当时这种方式肯定会有所牺牲并且需要进行单独的后台管理，如果后期这部分的修改需求较多，我们
可能会考虑采用动态的方式进行管理。

### 仓储实现
如果不使用我们其他的基础类库，则用户需要在仓储层实现类上除了需要实现具体的仓储接口外，还需要继承自`AbpRepositoryBase<TEntity, TPrimaryKey>`
虚类，以便于后期如果需要使用我们提供的类库降低切换的成本。

------

## 应用层（服务层）

### 应用接口
在完成领域层的功能之后我们还不能直接就开始编写具体的接口，这中间还需要应用层进行协调，而所有的应用层接口都需要继承自
`IApplicationService`接口，这样便于后期在应用层中增加通用服务等，同时也便于后期我们的程序集扫描。

### 应用实现
在我们实现具体的应用服务同时，除了需要实现具体业务的应用接口还需要继承公共的类库`ApplicationService`，以便提供公共的基础服务,
其中我们已经提供了部分服务，具体服务如下所示：
* `StringToGuid(s:String):Guid?`：将字符串转换为Guid类型，如果无法转换则返回Null。  

### 数据传输对象（DTO）
在表现层与应用层的交互过程中还存在一种模型用于进行数据的传输，在单一应用的情况下该DTO可以暂时共享领域模型，但是对于分布式的应用
或微服务来说，该对象需要独立编写，而在该类库中也提供了一套标准的对象，所有的应用接口的入参必须为对象且需要实现接口`IInputDto`，
如果该应用层接口恰好是需要获取的列表数据并需要分页那么我们也提供了对应的模型`PagedInputDto`其中规范了`Skip`和`Take`参数，避免
出现不同方式的分页。  
讲述完输入剩下的就是输出，输出的DTO必须继承自`OutputDto`，对于输出是列表的还需要配合`EntityPageIndexDto<TPrimaryKey>`和`IPagedResult<T>`
进行组织数据。  
但是随着我们后期全面采用gRpc进行通信，这些标准会采用`base.proto`进行表示，所以具体gRcp的DTO模型请根据实际情况做调整。

------

## 扩展过滤器  

### 标准输出  
为了保证系统平台的标准输出，框架提供了对应MVC过滤器将输出内容进行统一的格式化，最终格式化的输出可以参考`BaseResponse`类，具体如下
所示：  

```csharp
/// <summary>
/// 视图输出根类
/// </summary>
public class BaseResponse
{
    /// <summary>
    /// 错误消息
    /// </summary>
    public string errorMessage { get; set; }

    /// <summary>
    /// 错误代码
    /// </summary>
    public string errorCode { get; set; }

    /// <summary>
    /// 是否请求成功
    /// </summary>
    public bool success { get; set; }

    /// <summary>
    /// 数据对象
    /// </summary>
    public object data { get; set; }
}
```

根据用户的实际情况，可以采用特定`Action`进行标准格式化也可以进行全局配置，如果是特定的方式可以通过在我们需要进行标准格式输出的
`Action`上加上`StandardResultFilter`注解属性，比如下面这种使用方式：  

```csharp
[StandardResultFilter]
public IActionResult Get()
{
    // todo
}
```

如果需要默认全局则可以通过在`Startup`中在`AddMvc`中通过其`MvcOptions`进行添加比如以下方式我们就可以将标准输出
作为全局过滤器进行使用：  

```csharp
services.AddMvc(x =>
{
   x.Filters.AddStandardResultFilter(services);
});
```  

由于全局使用了标准输出，如果遇到不希望使用其过滤器的我们依然可以通过使用该过滤器在不希望使用的`Action`上进行排
除，比如下面就可以实现排除：  

```csharp
[StandardResultFilter(IsUse = false)]
public IActionResult Get()
{
    // todo
}
```

至此关于标准输出过滤器的使用方式就此结束。  

### 全局日志  

由于异常分析往往需要能够复现对应场景的情况，为此我们需要将对应请求的进参与出参进行记录以便于我们能够通过日志对其
进行更好的分析，为此我们在基于MVC上增加了对应的过滤器以实现该功能，当然具体的使用方式依然如同上述过滤器，依然可以
分为特定`Action`与全局，首先贴出针对性的使用方式：  

```csharp
[ActionLogFilter]
public IActionResult Get()
{
    // todo
}
```

如果需要全局都能够支持，按照之前的方式，我们需要在`Startup`中使用如下代码：  

```csharp
services.AddMvc(x =>
{
   x.Filters.AddActionLogFilter(services);
});
```

当然由于存在输入与输出，所以用户可以控制希望进行记录的日志，如果将输入与输出都关闭等于该过滤器不起任何作用，所以
我们可以针对性部分不需要记录日志的`Action`进行关闭：  

```csharp
[StandardResultFilter(Input = false, Output = false)]
public IActionResult Get()
{
    // todo
}
```

### 安全校验  

应用仅仅依赖传统的令牌并不能完全保证请求不被拦截并破坏后重播，为此我们需要在原本的请求的基础上增加额外的令牌以保
障请求即使在被拦截的情况依然可以提供有效的保障，为此我们在`MVC`增加了对应的过滤器，用户依然可以跟如上其他过滤器一
样使用全局或者部分，以下将会给出对应的代码：  

```csharp
[CheckSignatureFilter(Token = "abc")]
public IActionResult Get()
{
    // todo
}
```

对应全局使用方式如下：  

```csharp
services.AddMvc(x =>
{
   x.Filters.AddCheckSignatureFilter(services, "abc");
});
```

如果需要特定`Action`不使用则可以通过如下方式：  

```csharp
[CheckSignatureFilter(Use = false)]
public IActionResult Get()
{
    // todo
}
```

## 日志服务  

由于默认的日志框架的缺点，我们需要将日志通过`Logstash`传输到`ElasticSearch`上进行存储，所以我们需要将默认日志框架的背后实现逻辑采用
其他的日志框架进行替换，为此我们需要在项目中进行相关的配置以实现此目的，下面我们将介绍如何使用对应的日志框架。  

首先我们需要打开`Program`文件并使用`UseLog`使用对应日志框架：   

```csharp
var host = new WebHostBuilder()
    .UseKestrel()
    .UseUrls("http://*:5000")
    .UseStartup<Startup>()
    .UseLog()
    .Build();
```

完成以上配置仅仅只是将默认的日志框架进行了替换，我们还需要进行相关的配置以实现将我们需要的日志输出到对应的目标，所以我们需要先编写对应
的日志文件，如（nlog.config）具体还需要根据实际的运行环境进行调整：  

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      throwConfigExceptions="true"
      internalLogLevel="Debug"
      internalLogToTrace="true">
  <targets>
    <target name="lognet"
            xsi:type="Network"
             address="udp://10.247.107.52:4561" >
      <layout xsi:type="JsonLayout">
        <attribute name="type" layout="iotadapter" />
        <attribute name="date" layout="${longdate}" />
        <attribute name="level" layout="${level:uppercase=true}" />
        <attribute name="callSite" layout="${callsite:className=true:methodName=true:skipFrames=1}" />
        <attribute name="message" layout="${message}" />
        <attribute name="exception" layout="${exception:format=toString,Data}" />
        <attribute name="fileName" layout="${callsite:fileName=true:includeSourcePath=true}" />
      </layout>
    </target>
    <target name="console"
        xsi:type="ColoredConsole"
        layout="${longdate} [${level:uppercase=true}] ${callsite:className=true:methodName=true:skipFrames=1} ${message} ${exception:format=toString,Data} @${callsite:fileName=true:includeSourcePath=true}"/>
    <target xsi:type="Null" name="blackhole" />
  </targets>
  <rules>
    <logger name="Microsoft.*" minLevel="Trace" writeTo="blackhole" final="true" />
    <logger name="System.*" minLevel="Trace" writeTo="blackhole" final="true" />
    <logger name="*" minlevel="Error" writeTo="lognet,console" />
  </rules>
</nlog>
```

完成配置文件的编写后，我们需要告知日志系统对应的配置文件在哪，打开`Startup.cs`文件并在`Configure`方法中写入如下内容：  

```csharp
loggerFactory.ConfigureLog($"nlog.{env.EnvironmentName}.config");
```


#### 参数验证
为了提高开发效率我们把参数的自动化验证功能利用FluentValidation解决了，想要在项目中利用该特性需打开`Startup`文件中进行注册。  
比如以下代码为注册：  
`
services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ModelValidator>());
`

示例中的`ModelValidator`是参数验证类命名空间下的其中之一（`任选一个`注册即可）。  
`Model`模型类建议加上注解属性`[Validator(typeof(ModelValidator))]`。  
`ModelValidator`参数验证类`必须继承BaseRequestValidator`。  

`ModelValidator`参数验证方法中还可指定ErrorCode和ErrorMessage动态返回给调用方。  
以下代码为示例：  
`
RuleFor(x => x.Name).NotEmpty().WithErrorCode("-8").WithMessage("名称不能为空");
`

## 异常处理

#### 异常标准
对于系统校验等出现的异常，统一采用规定的异常进行输出，对应的中间件会对其进行处理以标准化的格式进行输出，这样可以避免组织大量的模型并且还要逐层返回，
其中已经制订的异常如下：
* `SinoException`：公共异常，对于没有对应定义的异常可以采用该类。
* `SinoNotFoundException`：数据不存在等异常。
* `SinoOperationException`：某种操作出现问题时的异常。
* `SinoAlreadyExistException`：已存在相同数据时的异常。

为了兼容后期gRpc的异常，所以在利用gRpc后对应的异常进行调整否则无法自然的传递到表现层，具体对应如下：
* `SinoRpcException`：公共异常，对于没有对应定义的异常可以采用该类。
* `SinoNotFoundRpcException`：数据不存在等异常。
* `SinoAlreadyExistRpcException`：数据已存在异常

如果要启用全局异常捕获需要在`Startup`中进行配置：  
`
app.UseGlobalExceptionHandler(loggerFactory);
`


## 其他
### JSON序列化
为了避免直接使用静态类，该类库中提供了`IJsonConvertProvider`接口将`JSON.NET`进行了封装，对于程序中需要使用到的地方请采用
该接口，该接口已经纳入IOC中，只需要在StartUp中进行添加即可：  
`
services.AddJson();
`

### 自动IOC注入
为了避免在后期大量的服务需要手动注入，利用`Scrutor`类库实现了自动根据名字进行IOC注入，默认规则为根据类名前加`I`的方式自动进行
注入，当然还有继承了接口`ISingletonDependency`和`ITransientDependency`才可以被自动注入，并按照规定的方式进行，以下就是如何使用的方式：
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    services.AutoDependency(typeof(IAInterface),typeof(IBInterface));
```

更详细的使用方式请参考[该文档](https://github.com/CSharpCross/Sino/blob/dev/docs/Sino_Web_Dependency.md)

### 验证自动注入
为了方便验证，这里将原本原始的初始化方式改为自动IOC注入的方式，对于需要验证的对象需要如下所示来对该实体模型进行验证：
```
public class UserValidator : BaseRequestValidator<User>
{
    public UserValidator()
    {
         RuleFor(x => x.UserName).NotEmpty().WithErrorCode("1000").WithMessage("用户名不能为空")
            .Length(0, 20).WithErrorCode("1001").WithMessage("用户名不能小于0或大于20");
    }
}
```
对于需要使用该验证的则通过以下方式注入：
```
public ValuesController(IBaseRequestValidator<User> validator)
{
    validator.ValidateModel(new Models.User
    {
         UserName = "25458545edfrdfedswaqw"
    });
}
```
新的验证已经内部集成了自动抛出异常，默认会将第一个异常抛出，所以务必对于每种验证加上`WithErrorCode`和`WithMessage`否则会导致
验证异常。  

## 文档版本
* 2017.3.9 v1.0 起草 by y-z-f
* 2017.3.28 v1.1 增加自动IOC扫描 by y-z-f
* 2017.3.31 v1.1.22-beta2 将项目移到src下 by y-z-f
* 2017.3.31 v1.1.22-beta3 增加支持IOC的参数验证 by y-z-f
* 2018.3.7 v2.0.0-beta1 支持Asp.net Core 2.0并去除gRpc支持 by y-z-f
* 2020.8.28

## 依赖类库
```
Dapper : 1.50.4   
FluentValidation : 7.5.0   
Microsoft.AspNetCore.Diagnostics : 2.0.0   
Microsoft.AspNetCore.Mvc.Core : 2.0.0    
Microsoft.Extensions.Caching.Abstractions : 2.0.0    
NLog : 4.5.0-rc06    
Scrutor : 2.2.1
```