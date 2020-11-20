using OrvilleX.EventBus.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OrvilleX.EventBus.Common
{
    /// <summary>
    /// 提供Event发送所携带的附加信息
    /// </summary>
    public class BasicPropertiesProvider : IBasicPropertiesProvider
    {
        private readonly RabbitMqConfiguration _config;

        public BasicPropertiesProvider(RabbitMqConfiguration config)
        {
            _config = config;
        }

        public IBasicProperties GetProperties<TMessage>(IModel model, Action<IBasicProperties> custom = null)
        {
            var properties = model.CreateBasicProperties();
            properties.MessageId = Guid.NewGuid().ToString();
            properties.Headers = new Dictionary<string, object>();
            properties.Persistent = _config.PersistentDeliveryMode;

            custom?.Invoke(properties);
            properties.Headers.Add(PropertyHeaders.Sent, DateTime.UtcNow.ToString("u"));
            properties.Headers.Add(PropertyHeaders.MessageType, GetTypeName(typeof(TMessage)));
            return properties;
        }

        private string GetTypeName(Type type)
        {
            var name = $"{type.Namespace}.{type.Name}";
            if (type.GenericTypeArguments.Length > 0)
            {
                var shouldInsertComma = false;
                name += '[';
                foreach (var genericType in type.GenericTypeArguments)
                {
                    if (shouldInsertComma)
                        name += ",";
                    name += $"[{GetTypeName(genericType)}]";
                    shouldInsertComma = true;
                }
                name += ']';
            }
            name += $", {type.GetTypeInfo().Assembly.GetName().Name}";
            return name;
        }
    }
}
