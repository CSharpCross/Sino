# ����ע��  

## ����֧������  

ͨ���滻ϵͳĬ�ϵ�IOC�Ӷ��ܹ�ʵ�ָ��ḻ�Ĺ��ܣ�Ϊ��������Ҫ�˽Ⲣ����ʹ��
��Щ�����ԣ������ĵ�����ͨ����ش���ʵ�����ܾ����ʹ�÷�ʽ��  

### �๹�캯��  

�ڴ����ʵ��ʹ���У����������Ὣ��Ҫע��Ķ���ͬ�����캯����һ�о٣���ֹ
����ע���޷�ѡ���ʺϵĹ��캯��������ȫ�µ�����ע���������Ǿ��ܹ�ʵ�ֶ�
���캯���������ݵ�ǰʵ���Ѵ��ڵĶ���ʹIOC����ѡ�񣬴Ӷ�ʵ�ָ��ӷḻ�Ĺ���
֧�֣����潫ͨ�������򵥵����ӽ���˵����  


�������Ƕ�����`A`��`B`��`C`�����࣬����`ServiceUser`������������ֹ��캯����  

```csharp
public ServiceUser(A a)
public ServiceUser(A a, B b) : this(a)
public ServiceUser(A a, B b, C c) : this(a, b)
```

��ʱ�����ǽ�`ServiceUser`ͨ��IOC����ʵ������ʱ����IOC����ݵ�ǰ�Ƿ�ע����
��Ӧ��`A`��`B`��`C`�������������Ǹ����캯������˼���ǵ�ǰ�������`A`��ע��
�ˣ���ô���ǵ���`public ServiceUser(A a)`���캯�����������`A`��`B`�������
`ServiceUser(A a, B b)`�ù��캯������Ȼ�������ֻ��`B`�࣬��Ȼ�Ͳ������κ�
���캯���ˡ�  

### ���������  

����ҵ��Ĳ��ϸ��ӻ����ܶೡ������Ҫ�Է�������һ��ȫ�ֵĴ���Ϊ�����Ǻܴ�̶�����Ҫ����AOP������
�������ǵ�����Ϊ�˸ÿ⵱ǰ�������������AOP������������һ�����������Դ�������ʵ��ҵ���������
�����ǽ��������ʹ����һ���ԡ�  

����������Ҫ��д��Ӧ��`Interceptor`������������Ҫ�̳�`SinoInterceptor`�ࣺ  

```csharp
	public class CalculatorInterceptor : SinoInterceptor
	{
	    protected override void PerformProceed(IInvocation invocation)
		{
		   // ������
			invocation.Proceed();
		}

		protected override void PreProceed(IInvocation invocation)
		{
		   // ����ǰ
		}

		protected override void PostProceed(IInvocation invocation)
		{
		   // ���ý�����
		}
	}
```  

������ʾ�����пɹ�������д�������෽����ÿ����������Ӧ��Ŀ�귽���Ĳ�ͬ�������ڡ����߿��Ը���ʵ��
����Ҫ��д��Ӧ�ķ����������Ҫ��ȡ����������ص���Ϣ�����ͨ��`invocation`��������ȡ�����������
Ҫ��`Startup`��`ConfigureServices`�����н���ӦAOP�ķ�������ע�룺  

```csharp
public IServiceProvider ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    var builder = services.CreateContainer();
	builder.AddInterceptor<SinoInterceptor>().AddInterceptor<SinoInterceptor>("FooInterceptor");
    return builder.Build();
}
```  

�������ע�⵽ע��ķ�ʽ�������֣�������������ע��ʹ�������ע�롣�����Ҫ˵����ξ������巽��
�ˣ�������Ҫʹ��`SinoInterceptorAttribute`ע��������ʵ�����룬����ע������д����Ӧ�ķ����ϲ���
�����ͻ������Ƶķ�ʽ���й����������·�ʽ��  

```csharp
[SinoInterceptor("fooInterceptor")]
[SinoInterceptor(typeof(SinoInterceptor))]
public class CalculatorServiceWithStandartInterceptorTwo : CalculatorService
{

}
```  

��ɺ󵱷�������IOC�����÷����ú����ǵ�AOP�Ϳ����������ˡ�  

