using RabbitMQ.Client;
using System;

namespace OrvilleX.EventBus.Common
{
    /// <summary>
    /// 提供Event发送所携带的附加信息接口
    /// </summary>
    public interface IBasicPropertiesProvider
    {
        IBasicProperties GetProperties<TMessage>(IModel model, Action<IBasicProperties> custom = null);
    }
}
