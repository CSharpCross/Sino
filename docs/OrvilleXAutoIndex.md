# Sino.Extensions.AutoIndex ʹ�ý���  

�ÿ��ǻ��ڵεε�[tinyid](https://github.com/didi/tinyid)�����Ļ���`.Net Core`ƽ̨��
SDK���ÿ�ο���ԭJavaƽ̨SDK������������Ծ���ʵ�֡������Ҫʹ����ͨ�����·�ʽ���а�װ��  

```
Install-Package Sino.Extensions.AutoIndex
```  

## ��ʵʹ��  

��ɰ�װ�����ǻ���Ҫ��`Startup`�н���ע�룬�������Ǵ��ļ�����`ConfigureServices`������
д�����´��룺  

```csharp
services.AddTinyId(Configuration.GetSection("tinyid"));
```  

������������Բο�`TinyIdClientConfiguration`�������д��  


���ע�������Ҫʹ�õĵط�����ͨ��ע��`ITinyIdClient`�ӿ�ʵ��ʹ�ã��ýӿ���Ҫ�ṩ����������
�ӿ��ṩ��ط����֧�֣�  

```csharp
    /// <summary>
    /// Tiny�ͻ��˽ӿ�
    /// </summary>
    public interface ITinyIdClient
    {
        /// <summary>
        /// �����ʶ��
        /// </summary>
        /// <param name="bizType">���</param>
        Task<long> NextId(string bizType);

        /// <summary>
        /// ���������ʶ��
        /// </summary>
        /// <param name="bizType">���</param>
        /// <param name="batchSize">��������</param>
        Task<IList<long>> NextId(string bizType, int batchSize);
    }
```  

## ����  

���ڹ�˾�������������˻���Docker�Ĳ���ʽ�����Ա������Լ��Ĵ���ֿ��������˶�Ӧ`Dockerfile`
�ļ���֧�ֻ��������Ĳ���ʽ[�ֿ��ַ](https://github.com/CSharpCross/tinyid)��������˾�ڲ���
ͬѧ�����ֱ��ͨ�����·�ʽ���ؾ��񲢽������У�  

```bash
docker pull harbor.vip56.cn/common/tinyid:1.0
docker run -p 80:80 -e SPRING_PROFILES_ACTIVE=test -it --name tinyid tinyid:1.0
```  

����ע����Ҫ��ǰ���½���Ӧ���ݿⲢ��`tinyid-server/db.sql`����ִ���Դ�����Ӧ��ID�����Լ�����
������Ϣ������ID������Ҫע�����¼���������  

* max_id����ʼ��ҪΪ1����ÿ������֮������`step`���е���  
* step������������ͻ���ÿ�λ�ȡ��ID���䷶Χ  
* delta��ID�����������ͻ���ÿ�η�����ڵ�ǰID��������  
* remainder�����������ڶ�DB  

�����Լ������ӽ���˵������������BDϣ������1,2,3,4�������е�ID���������DB��Ӧ�Ĳ�����Ҫ���£�  

* DB1��delta=2, remainder=0  
* DB2��delta=2, remainder=1  

���ڿͻ���SDK������˫Buffer�ķ�ʽ����ID��Դ���Ĺ�������������`step`���ù�С�����򽫵��¶���
������������ʱ��������ʱ��������ID�������ֱ�ӵ��±���ID��Դ������ǿ�Ƽ��أ��Ӷ�������ID��
�������������˷ѵ�ID��Դ��  

## ��׼����  

���λ�׼���ԵĻ������£�  

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1082 (1909/November2018Update/19H2)
Intel Core i7-8700 CPU 3.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.1.402
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), X64 RyuJIT
  Job-SCYTYU : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), X64 RyuJIT

Runtime=.NET Core 2.0  RunStrategy=Throughput  

```  

���½���ȡ�������ν�����л㱨��  

* ���1  

|       Method |         Mean |      Error |     StdDev |          Min |          Max |       Median |
|------------- |-------------:|-----------:|-----------:|-------------:|-------------:|-------------:|
| SingleNextId |     1.602 ��s |  0.0256 ��s |  0.0239 ��s |     1.563 ��s |     1.642 ��s |     1.599 ��s |
|  BatchNextId | 1,517.713 ��s | 15.6616 ��s | 13.0782 ��s | 1,496.979 ��s | 1,542.687 ��s | 1,520.472 ��s |

* ���2  

|       Method |         Mean |      Error |     StdDev |          Min |          Max |       Median |
|------------- |-------------:|-----------:|-----------:|-------------:|-------------:|-------------:|
| SingleNextId |     1.584 ��s |  0.0159 ��s |  0.0149 ��s |     1.561 ��s |     1.612 ��s |     1.583 ��s |
|  BatchNextId | 1,567.847 ��s | 18.6749 ��s | 16.5549 ��s | 1,542.427 ��s | 1,599.790 ��s | 1,570.855 ��s |

* ���3  

|       Method |         Mean |      Error |     StdDev |          Min |          Max |       Median |
|------------- |-------------:|-----------:|-----------:|-------------:|-------------:|-------------:|
| SingleNextId |     1.596 ��s |  0.0251 ��s |  0.0235 ��s |     1.558 ��s |     1.636 ��s |     1.594 ��s |
|  BatchNextId | 1,559.060 ��s | 23.6504 ��s | 20.9654 ��s | 1,518.869 ��s | 1,591.075 ��s | 1,560.865 ��s |
