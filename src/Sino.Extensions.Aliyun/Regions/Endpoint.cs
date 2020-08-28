using System.Collections.Generic;

namespace Sino.Extensions.Aliyun.Regions
{
    public class Endpoint
    {
        public Endpoint(string name, ISet<string> regionIds, List<ProductDomain> productDomains)
        {
            Name = name;
            RegionIds = regionIds;
            ProductDomains = productDomains;
        }

        public string Name { get; private set; }
        public ISet<string> RegionIds { get; private set; }
        public List<ProductDomain> ProductDomains { get; private set; }

        public static ProductDomain FindProductDomain(string regionId, string product, List<Endpoint> endpoints)
        {
            if (null == regionId || null == product || null == endpoints)
            {
                return null;
            }

            foreach (var endpoint in endpoints)
            {
                if (endpoint.RegionIds.Contains(regionId))
                {
                    var domain = FindProductDomainByProduct(endpoint.ProductDomains, product);
                    return domain;
                }
            }

            return null;
        }

        private static ProductDomain FindProductDomainByProduct(List<ProductDomain> productDomains, string product)
        {
            if (null == productDomains)
            {
                return null;
            }

            foreach (var productDomain in productDomains)
            {
                if (product.Equals(productDomain.ProductName))
                {
                    return productDomain;
                }
            }

            return null;
        }
    }
}
