using Microsoft.Extensions.Options;

namespace Sino.Extensions.Redis
{
    /// <summary>
    /// 配置项
    /// </summary>
    public class RedisCacheOptions : IOptions<RedisCacheOptions>
    {
        /// <summary>
        /// 是否在不存在可用的服务资源时不创建连接
        /// </summary>
        public bool AbortOnConnectFail { get; set; } = true;

        /// <summary>
        /// 是否允许调用存在风险的操作
        /// </summary>
        public bool AllowAdmin { get; set; } = false;

        /// <summary>
        /// 连接重试次数
        /// </summary>
        public int ConnectRetry { get; set; } = 3;

        /// <summary>
        /// 连接重试时间
        /// </summary>
        public int ConnectTimeout { get; set; } = 5000;

        /// <summary>
        /// 数据库索引
        /// </summary>
        public int DefaultDatabase { get; set; } = 0;

        /// <summary>
        /// 是否启用加密通道
        /// </summary>
        public bool Ssl { get; set; } = false;

        /// <summary>
        /// 加密通信地址
        /// </summary>
        public string SslHost { get; set; }

        /// <summary>
        /// 加密通信端口
        /// </summary>
        public int SslPort { get; set; }

        /// <summary>
        /// 通信地址
        /// </summary>
        public string Host { get; set; } = "localhost";

        /// <summary>
        /// 通信端口
        /// </summary>
        public int Port { get; set; } = 6379;

        /// <summary>
        /// Redis 6开启ACL后的用户名
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Key固定前缀
        /// </summary>
        public string InstanceName { get; set; }

        RedisCacheOptions IOptions<RedisCacheOptions>.Value
        {
            get { return this; }
        }
    }
}
