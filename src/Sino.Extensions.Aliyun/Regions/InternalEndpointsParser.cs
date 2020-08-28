using Sino.Extensions.Aliyun.Auth;
using Sino.Extensions.Aliyun.Regions.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.Aliyun.Regions
{
    public class InternalEndpointsParser : IEndpointsProvider
    {
        private readonly IDictionary<string, string> globalEndpointCollection;

        private readonly IDictionary<string, string> regionIdEndpointCollection;

        public InternalEndpointsParser()
        {
            regionIdEndpointCollection = EndpointResource.RegionalEndpoints;
            globalEndpointCollection = EndpointResource.GlobalEndpoints;
        }

        public Endpoint GetEndpoint(string regionId, string productName)
        {
            string domain;

            regionIdEndpointCollection.TryGetValue(string.Format("{0}_{1}", productName.ToLower(), regionId), out domain);

            if (string.IsNullOrEmpty(domain))
            {
                globalEndpointCollection.TryGetValue(productName.ToLower(), out domain);
            }

            if (string.IsNullOrEmpty(domain))
            {
                return null;
            }

            var regionHashset = new HashSet<string> { regionId };
            var productDomain = new List<ProductDomain> { new ProductDomain(productName, domain) };

            return new Endpoint(productName, regionHashset, productDomain);
        }

        public Endpoint GetEndpoint(string region, string product, string serviceCode, string endpointType,
            AliyunCredential credential, LocationConfig locationConfig)
        {
            throw new NotSupportedException();
        }
    }
}
