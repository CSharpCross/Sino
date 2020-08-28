using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.Aliyun.Regions
{
    /// <summary>
    /// 产品域
    /// </summary>
    public class ProductDomain
    {
        public ProductDomain()
        {
        }

        public ProductDomain(string product, string domain)
        {
            ProductName = product;
            DomainName = domain;
        }

        public string ProductName { get; set; }
        public string DomainName { get; set; }
    }
}
