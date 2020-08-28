using Sino.Extensions.Aliyun.Auth;
using Sino.Extensions.Aliyun.Regions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Sino.Extensions.Aliyun.Http
{
    public class AcsRequest
    {
        public FormatType AcceptFormat { get; set; }

        public ProtocolType Protocol { get; set; }

        public string ProductEndpointType { get; set; }

        public Dictionary<string, string> ProductEndpointMap { get; set; }

        public string RegionId { get; set; }

        public string Product { get; set; }

        public string ProductSuffix { get; set; }

        public string ProductNetwork { get; set; }

        public ProductDomain ProductDomain { get; set; }

        public UserAgent UserAgentConfig { get; set; } = new UserAgent();

        public void SetEndpoint(string endpoint)
        {
            ProductDomain = new ProductDomain { ProductName = Product, DomainName = endpoint };
        }

        public void SetProductDomain(string endpoint = "")
        {
            if (endpoint == "")
            {
                endpoint = GetProductEndpoint();
            }

            if (endpoint != "" && ProductDomain == null)
            {
                ProductDomain = new ProductDomain { ProductName = Product, DomainName = endpoint };
            }
        }

        public string GetProductEndpoint()
        {
            if (ProductEndpointMap == null && ProductEndpointType == null)
            {
                return "";
            }

            foreach (var endpointItem in ProductEndpointMap)
            {
                if (endpointItem.Key == RegionId)
                {
                    return endpointItem.Value;
                }
            }

            var endpoint = "";
            if (ProductEndpointType == "central")
            {
                endpoint = "<product_id><suffix><network>.aliyuncs.com";
            }
            else if (ProductEndpointType == "regional")
            {
                endpoint = "<product_id><suffix><network>.<region_id>.aliyuncs.com";
                endpoint = endpoint.Replace("<region_id>", RegionId);
            }

            if (string.IsNullOrWhiteSpace(ProductSuffix))
            {
                endpoint = endpoint.Replace("<suffix>", string.Empty);
            }
            else
            {
                endpoint = endpoint.Replace("<suffix>", ProductSuffix);
            }

            if (endpoint == "")
            {
                return "";
            }

            endpoint = endpoint.Replace("<product_id>", Product.ToLower());

            endpoint = ProductNetwork == "public" ?
                endpoint.Replace("<network>", "") :
                endpoint.Replace("<network>", "-" + ProductNetwork);

            return endpoint;
        }

        public HttpRequestMessage SignRequest(Signer signer, AliyunCredential credential,
    FormatType? format, ProductDomain domain)
        {
            return SignRequest(signer, new LegacyCredentials(credential), format, domain);
        }

        public abstract HttpRequestMessage SignRequest(Signer signer, AlibabaCloudCredentials credentials,
            FormatType? format, ProductDomain domain);
    }
}
