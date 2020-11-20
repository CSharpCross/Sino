# ����ʹ��  

### 1. ����  

��`appsettings.json`����������������Ϣ��  

```json
  "EventBus": {
    "Username": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "Port": 5672,
    "Hostnames": [ "localhost" ],
    "PublishConfirmTimeout": "00:00:01",
    "RecoveryInterval": "00:00:10",
    "PersistentDeliveryMode": true,
    "AutoCloseConnection": true,
    "AutomaticRecovery": true,
    "TopologyRecovery": true,
    "Exchange": {
      "Durable": true,
      "AutoDelete": true,
      "Type": "Topic"
    },
    "Queue": {
      "AutoDelete": true,
      "Durable": true,
      "Exclusive": true
    }
  }
```  

��������˵��������ʾ��  

- `Username`���û���  
- `Password`������  
- `VirtualHost`������·��  
- `Port`���˿�  
- `Hostnames`:������ַ�б�  
- `PublishConfirmTimeout`���ȴ�������ȷ�ϵĳ�ʱʱ��  
- `RecoveryInterval`���Զ����Լ��  
- `PersistentDeliveryMode`���־û����ԣ���Ϣ�ǻ����ڴ滹��Ӳ�̴洢����������ܵ����������Ϣ���ȶ������������ΪFalse  
- `AutoCloseConnection`���Ƿ�������ͨ���رպ��Զ��ر�����  
- `AutomaticRecovery`���Ƿ������Զ��ָ� (����, ͨ���ؿ�, �޸�QoS)  
- `TopologyRecovery`���Ƿ�����topology�ָ� (���������������Ͷ���, �޸��󶨺�������)  
- `Exchange Durable`���������Ƿ�־û�����ҪQueueҲΪ�־û�ͬʱ��Ϣ����ʱDeliveryModeΪ2�ſ��ã������Խ��ή��RabbitMQ���ܣ�  
- `Exchange AutoDelete`���Ƿ������ж��н���ʱ�Զ�ɾ��������  
- `Exchange Type`������������  
- `Queue AutoDelete`�������в������κ�������ʱ���Ƿ��Զ�ɾ��  
- `Queue Durable`���־û�  
- `Queue Exclusive`��ר�ö���  

### 2. �¼�����  

�¼����鵥��һ�������ж��壬���������ڽ�������nuget�������������Ŀֱ�����ã�ͬʱ�ڸ������Ŀ��Ӧ�ö���һ�������Ļ����¼���
��`BaseEvent`�����п��Զ���һ����ͨ�õ�ʱ�����Ա��緢��ʱ�䣬��Ϊ���е��¼�����̳���`IAsyncNotification`�ӿڣ���������Ҳ����
����������������¼����ࣺ  

```csharp
public abstract class BaseEvent : IAsyncNotification
{
    public BaseEvent()
    {
        Time = DateTime.Now;
    }
    public DateTime Time { get; set; }
}
```

������ҵ���¼��������`����+����+Event`�ķ�ʽ�����������������Թ淶����ϵͳ���¼������㿴�����¼������⡣    

### 3. �ͷ��ˣ����ͷ���  

��Ϊ���պͷ��Ͷ��������������У����Զ�Ӧ��������һ���Ĳ�࣬������Կͻ��ˣ����ͷ���Ϊ������ʾ��γ�ʼ��������ĳ�ʼ��
����ASP.NET CoreΪ׼�����ȴ�`Startup`�ļ�����`ConfigureServices`�������������ݣ�  

```csharp
services.AddEventBus(Configuration.GetSection("EventBus"));
```

�����ǵĴ�����ֱ�ӿ���ͨ������`IEventBus`�ӿڼ���ʹ�ã���������Ĵ������`Controller`�з�����һ���¼���  

```csharp
public OrderController(IEventBus eventBus)
{
    _eventBus = eventBus;
}

[HttpGet("Add")]
public IActionResult Add()
{
    _eventBus.PublishAsync(new AddOrderEvent()
    {
        Id = 1,
        Title = "����",
        Count = 1,
        UserId = 2
    });
    return Ok();
}
```  

### 4. ����ˣ����շ���  

���շ���Ϊ�����Զ�IOC�Ĳ����Լ��ֶ�ָ���Ĳ��֣���Ӧ������Ҳ��һЩ��ͬ���Ǵ�`Startup`�ļ�����`ConfigureServices`��
�������´��룺  

```csharp
services.AddEventBus(Configuration.GetSection("EventBus"), typeof(Startup));
```  

���еڶ���������Ĳ�����ָ��Event����������ڳ����е�����һ����������ͼ��ɣ���Ϊ�ڲ���Ҫɨ��������򼯡������
��ĳ�ʼ�����������еĴ�����򶼻�ע�ᵽIOC�У��������ʱ����Щ�������û�п��ã���Ϊ���ǵ�һ�����������⣬����
���ձ���ͨ����һ�����ò���������ĳһ��Event�ļ�����������������Ҫ����`AddOrderEvent`��ô����Ҫ��`Configure`����
�����´��룺  

```csharp
app.AddHandler<AddOrderEvent>();
```  

### 5. �¼��������  

���е��¼������������¼�û�в���ע�����Եķ�ʽ�涨Exchange��Queue��Routing Key�ı�������Event����ĳ��򼯣���
�������޷��յ������⣬֮�����ǿ���ͨ��ʵ��`IAsyncNotificationHandler<>`���ͽӿ�������ָ����Event����Ȼ�����ڲ�
ʹ�����Դ���IOC����ʵ�ָýӿڵ���������ʹ��������Ŀ�������Ѿ����õ�IOC�еķ��񣬱����������������Ǵ���`AddOrde
rEvent`�Ĵ������  

```csharp
public class AddOrderEventHandler : IAsyncNotificationHandler<AddOrderEvent>
{
    public Task Handle(AddOrderEvent notification)
    {
        return Task.CompletedTask;
    }
}
```  

### ע������  

�ṩ������������ע������`ExchangeAttribute`��`QueueAttribute`��`RoutingAttribute`����Щע�����Կ��Զ����ڶ�Ӧ��
`Event`���ϼ��ɣ����п������ô󲿷ֵ����ò�����  

### �Զ���������  

- ������������`��������`��`[�¼�����]_[������Ŀ]`���ֽṹ���ɣ�����������Ŀ��Ubuntu 16.04�ϵ�Docker�а��������������������·��
- `RoutingKey`�����¼���������
- `������`�����¼������������ռ����  

# �߼��÷�  

----

### �Զ���ͻ��˲���  

������Ҫ�Զ���һЩ����ͻ��˲�������RabbitMQ����������ʵ�ֽӿ�`IClientPropertyProvider`��̳�`ClientPropertyProvider`�е�`GetC
lientProperties`������Ȼ������IOC����ע�뼴�ɡ���Ĭ�ϵĿͻ��˲������£�  

```csharp
var props = new Dictionary<string, object>
{
    { "product", "EventBus" },
    { "version", typeof(EventBus).GetTypeInfo().Assembly.GetName().Version.ToString() },
    { "platform", "corefx" }
};
return props;
```  

### �Զ������л���ʽ  

Ĭ���ǲ���JSON�ķ�ʽ�������л�������û���Ҫ����������ʽ�������л�����ͨ��ʵ��`IMessageSerializer`�ӿڲ�ͨ��IOCע�뼴�ɣ���
��Ĭ�ϵ�`Newtonsoft.Json`���������£�  

```csharp
_serializer = new JsonSerializer
{
    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
    Formatting = Formatting.None,
    CheckAdditionalContent = true,
    ContractResolver = new CamelCasePropertyNamesContractResolver(),
    ObjectCreationHandling = ObjectCreationHandling.Auto,
    DefaultValueHandling = DefaultValueHandling.Ignore,
    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
    MissingMemberHandling = MissingMemberHandling.Ignore,
    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
    NullValueHandling = NullValueHandling.Ignore
};
```  

### �Զ���Event������Ϣ  

��Ҫ��Ҫ��ÿ��Event�����Ӹ��ӵ����ݣ��û������Լ�ʵ�ֽӿ�`IBasicPropertiesProvider`��ͨ��IOCע�뼴�ɣ�Ĭ���ǰ������¶��������  

- `sent`������ʱ��UTCʱ��
- `message_type`���¼�����������������  

### �Զ�������  

���ʵ�ʵ�ʹ�ù����д��ڸߵͷ����������ͨ�������Զ����������ﵽ����Ҫ�������ݵ�ʱ��ʱ��ͨ����������ȥ�������ڵ͹��ڵ�ʱ
�����ݴﵽ��ʡ��Դ�����á�  Ĭ���������ǲ��������Զ������ݵģ�����û���Ҫʹ�ÿ���ͨ������`ChannelFactoryConfiguration`ע��
��IOC�м��ɣ����ж�Ӧÿ��������˵��������ʾ��  

- `EnableScaleUp`���Ƿ������Զ�����  
- `EnableScaleDown`���Ƿ������Զ�����  
- `ScaleInterval`���Զ������ݵ�ɨ����ʱ��  
- `GracefulCloseInterval`��ƽ���ر�ͨ���ļ��ʱ��  
- `MaxChannelCount`�����Ŀɴ�����ͨ������  
- `InitialChannelCount`����ʼ������ͨ������  
- `WorkThreshold`��ָ��ÿ��ͨ������Ϣ�ﵽ�ñ�ֵ�Ż�����  

### ����ElasticSearch��־��¼  

��Ϊ�������������ҵ�����������־��¼��Ҫ�����ڳ������־��¼��Ϊ�˼�¼��������Ĺ�������֤�ܹ������ĸ������������û�
��[NLog](http://nlog-project.org/)��[ElasticSearch](https://github.com/ReactiveMarkets/NLog.Targets.ElasticSearch)����չ
��֧�֣�������ԭ����`nlog.config`�����Ӷ�Ӧ��Ŀ�����ã�  

```xml
<extensions>
    <add assembly="NLog.Targets.ElasticSearch"/>
</extensions>
<target name="elastic" xsi:type="BufferingWrapper" flushTimeout="5000">
    <target xsi:type="ElasticSearch"
        name="elastic"
        uri="http://127.0.0.1:9200"
        index="eventbus"
        documentType="log"
        includeAllProperties="false"
        layout="${longdate} [${level:uppercase=true}] ${callsite:className=true:methodName=true:skipFrames=1} ${message} ${exception:format=toString,Data} @${callsite:fileName=true:includeSourcePath=true}" />
</target>
```  

����`BufferingWrapper`�������𵽻������õģ������е��ӽڵ�����ص�Ĳ����ˣ���Ӧ������������ʾ��  

- `uri`��ElasticSearch�ĵ�ַ
- `index`������������
- `documentType`���ĵ�����
- `includeAllProperties`���Ƿ������������  

�������ǻ���Ҫ������־��ֻ�����ǵĻ����������־����������ǻ����������������`Sino.Extensions`��ͷ�ģ������������������ף�  

```xml
<logger name="Sino.Extensions.*" minlevel="Debug" writeTo="console,elastic" />
```  

�������ǵĻ�������Ϳ��Զ����ļ�¼�ˡ�   

ע�⣬��������Ϊ�ǻ���1.1.*�汾�Ŀ��д��Ϊ���ܹ��������ǵ�ǰ����Ŀ�汾�����Խ����˽������ϴ����������Լ���˽�п⣬����ֱ�Ӱ�װ�����ϵĿ⡣  

```
Install-Package NLog.Targets.ElasticSearch
```   

## ע������  

- ��`Exchange AutoDelete`Ϊ`True`������£���������¼�Ƶ��������RabbitMQ������Ƶ�ԭ����ܻᵼ�´����������ȳ���TimeOut�쳣�����齫�����ø�
Ϊ`False`�Ӷ����ó�����Ļ��湦�ܣ�����ÿ�η����¼�����⽻������  