using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.AutoIndex
{
    /// <summary>
    /// ID生成器配置项
    /// </summary>
    public class TinyIdClientConfiguration
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 服务器列表
        /// </summary>
        public List<string> Servers { get; set; }
    }
}
