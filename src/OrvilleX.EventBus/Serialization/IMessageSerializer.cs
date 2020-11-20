using RabbitMQ.Client.Events;
using System;

namespace OrvilleX.EventBus.Serialization
{
    /// <summary>
    /// 序列化标准接口，便于以后切换序列化的方式。
    /// </summary>
    public interface IMessageSerializer
    {
        byte[] Serialize<T>(T obj);
        T Deserialize<T>(byte[] bytes);
        object Deserialize(byte[] bytes, Type messageType);
        object Deserialize(BasicDeliverEventArgs args);
    }
}
