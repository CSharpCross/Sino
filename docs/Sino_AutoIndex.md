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
