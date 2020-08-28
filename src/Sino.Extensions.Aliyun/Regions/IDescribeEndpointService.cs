using Sino.Extensions.Aliyun.Auth;
using Sino.Extensions.Aliyun.Regions.Locations;

namespace Sino.Extensions.Aliyun.Regions
{
    public interface IDescribeEndpointService
    {
        DescribeEndpointResponse DescribeEndpoint(string regionId, string serviceCode, string endpointType,
            AliyunCredential credential,
            LocationConfig locationConfig);
    }
}
