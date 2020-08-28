namespace Sino.Extensions.Aliyun.Regions.Locations
{
    public class LocationConfig
    {
        public LocationConfig() { }

        public LocationConfig(string regionId, string product, string endpoint)
        {
            RegionId = regionId;
            Product = product;
            Endpoint = endpoint;
        }

        public static LocationConfig CreateLocationConfig(string regionId, string product, string endpoint)
        {
            return new LocationConfig(regionId, product, endpoint);
        }

        public string RegionId { get; set; } = "cn-hangzhou";

        public string Product { get; set; } = "Location";

        public string Endpoint { get; set; } = "location.aliyuncs.com";
    }
}
