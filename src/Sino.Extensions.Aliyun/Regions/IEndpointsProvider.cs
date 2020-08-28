using Sino.Extensions.Aliyun.Auth;
using Sino.Extensions.Aliyun.Regions.Locations;

namespace Sino.Extensions.Aliyun.Regions
{
    /// <summary>
    /// 访问端点提供器
    /// </summary>
    public interface IEndpointsProvider
    {
        Endpoint GetEndpoint(string region, string product);

        Endpoint GetEndpoint(string region, string product, string serviceCode, string endpointType,
            AliyunCredential credential,
            LocationConfig locationConfig);
    }
}
