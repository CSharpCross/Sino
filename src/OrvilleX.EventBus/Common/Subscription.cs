using OrvilleX.EventBus.Consumer;
using RabbitMQ.Client;

namespace OrvilleX.EventBus.Common
{
    public class Subscription : ISubscription
    {
        public string QueueName { get; }
        public string[] ConsumerTags { get; }
        public bool Active { get; set; }

        private readonly IRawConsumer _consumer;

        public Subscription(IRawConsumer consumer, string queueName)
        {
            _consumer = consumer;
            var basicConsumer = consumer as DefaultBasicConsumer;
            if (basicConsumer == null)
            {
                return;
            }
            QueueName = queueName;
            ConsumerTags = basicConsumer.ConsumerTags;
        }

        public void Dispose()
        {
            Active = false;
            _consumer.Disconnect();
        }
    }
}
