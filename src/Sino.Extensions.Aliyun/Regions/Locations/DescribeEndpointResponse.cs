using Sino.Extensions.Aliyun.Http;

namespace Sino.Extensions.Aliyun.Regions.Locations
{
    public class DescribeEndpointResponse : AcsResponse
    {
        public string Endpoint { get; set; }
        public string RegionId { get; set; }
        public string Product { get; set; }
    }
}
