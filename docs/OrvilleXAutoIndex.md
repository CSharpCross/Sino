# Sino.Extensions.AutoIndex 使用介绍  

该库是基于滴滴的[tinyid](https://github.com/didi/tinyid)开发的基于`.Net Core`平台的
SDK，该库参考了原Java平台SDK，所以相关特性均以实现。如果需要使用请通过如下方式进行安装：  

```
Install-Package Sino.Extensions.AutoIndex
```  

## 如实使用  

完成安装后我们还需要在`Startup`中进行注入，首先我们打开文件并在`ConfigureServices`方法中
写入如下代码：  

```csharp
services.AddTinyId(Configuration.GetSection("tinyid"));
```  

其中配置项可以参考`TinyIdClientConfiguration`类进行填写。  


完成注入后，在需要使用的地方可以通过注入`ITinyIdClient`接口实现使用，该接口主要提供了如下两个
接口提供相关服务的支持：  

```csharp
    /// <summary>
    /// Tiny客户端接口
    /// </summary>
    public interface ITinyIdClient
    {
        /// <summary>
        /// 请求标识符
        /// </summary>
        /// <param name="bizType">类别</param>
        Task<long> NextId(string bizType);

        /// <summary>
        /// 批量请求标识符
        /// </summary>
        /// <param name="bizType">类别</param>
        /// <param name="batchSize">请求数量</param>
        Task<IList<long>> NextId(string bizType, int batchSize);
    }
```  

## 服务搭建  

由于公司大多数服务采用了基于Docker的部署方式，所以笔者在自己的代码仓库中增加了对应`Dockerfile`
文件以支持基于容器的部署方式[仓库地址](https://github.com/CSharpCross/tinyid)，对于我司内部的
同学则可以直接通过以下方式下载镜像并进行运行：  

```bash
docker pull harbor.vip56.cn/common/tinyid:1.0
docker run -p 80:80 -e SPRING_PROFILES_ACTIVE=test -it --name tinyid tinyid:1.0
```  

这里注意需要提前将新建对应数据库并将`tinyid-server/db.sql`进行执行以创建对应的ID规则以及访问
令牌信息，其中ID规则需要注意以下几个参数：  

* max_id：初始需要为1，在每次申请之后会根据`step`进行递增  
* step：步进，代表客户端每次获取的ID区间范围  
* delta：ID递增数，即客户端每次分配基于当前ID递增多少  
* remainder：余数，用于多DB  

以下以几个列子进行说明，比如两个BD希望生成1,2,3,4这样序列的ID编号则两个DB对应的参数需要如下：  

* DB1：delta=2, remainder=0  
* DB2：delta=2, remainder=1  

由于客户端SDK采用了双Buffer的方式，在ID资源消耗过快的情况下切勿将`step`设置过小，否则将导致二级
缓存进行请求的时候（网络延时），由于ID请求过快直接导致本地ID资源消耗完强制加载，从而出现跳ID段
的情况，极大的浪费的ID资源。  

## 基准测试  

本次基准测试的环境如下：  

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1082 (1909/November2018Update/19H2)
Intel Core i7-8700 CPU 3.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.1.402
  [Host]     : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), X64 RyuJIT
  Job-SCYTYU : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), X64 RyuJIT

Runtime=.NET Core 2.0  RunStrategy=Throughput  

```  

以下将截取其中三次结果进行汇报：  

* 结果1  

|       Method |         Mean |      Error |     StdDev |          Min |          Max |       Median |
|------------- |-------------:|-----------:|-----------:|-------------:|-------------:|-------------:|
| SingleNextId |     1.602 μs |  0.0256 μs |  0.0239 μs |     1.563 μs |     1.642 μs |     1.599 μs |
|  BatchNextId | 1,517.713 μs | 15.6616 μs | 13.0782 μs | 1,496.979 μs | 1,542.687 μs | 1,520.472 μs |

* 结果2  

|       Method |         Mean |      Error |     StdDev |          Min |          Max |       Median |
|------------- |-------------:|-----------:|-----------:|-------------:|-------------:|-------------:|
| SingleNextId |     1.584 μs |  0.0159 μs |  0.0149 μs |     1.561 μs |     1.612 μs |     1.583 μs |
|  BatchNextId | 1,567.847 μs | 18.6749 μs | 16.5549 μs | 1,542.427 μs | 1,599.790 μs | 1,570.855 μs |

* 结果3  

|       Method |         Mean |      Error |     StdDev |          Min |          Max |       Median |
|------------- |-------------:|-----------:|-----------:|-------------:|-------------:|-------------:|
| SingleNextId |     1.596 μs |  0.0251 μs |  0.0235 μs |     1.558 μs |     1.636 μs |     1.594 μs |
|  BatchNextId | 1,559.060 μs | 23.6504 μs | 20.9654 μs | 1,518.869 μs | 1,591.075 μs | 1,560.865 μs |
