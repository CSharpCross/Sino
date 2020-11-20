using OrvilleX.EventBus.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace OrvilleX.EventBus.Common
{
    /// <summary>
    /// 为连接到RabbitMQ提供自定义客户端参数
    /// </summary>
    public class ClientPropertyProvider : IClientPropertyProvider
    {
        public IDictionary<string, object> GetClientProperties(RabbitMqConfiguration cfg = null)
        {
            var props = new Dictionary<string, object>
            {
                { "product", "EventBus" },
                { "version", typeof(EventBus).GetTypeInfo().Assembly.GetName().Version.ToString() },
                { "platform", "corefx" }
            };
            return props;
        }
    }
}
