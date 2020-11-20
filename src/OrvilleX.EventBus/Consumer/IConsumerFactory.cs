using OrvilleX.EventBus.Configuration;
using RabbitMQ.Client;

namespace OrvilleX.EventBus.Consumer
{
    public interface IConsumerFactory
    {
        IRawConsumer CreateConsumer(IConsumerConfiguration cfg, IModel channel);
    }
}
