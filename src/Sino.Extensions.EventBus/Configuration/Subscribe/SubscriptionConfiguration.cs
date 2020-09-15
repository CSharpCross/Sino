﻿namespace Sino.Extensions.EventBus.Configuration
{
    public class SubscriptionConfiguration : IConsumerConfiguration
    {
        public bool NoAck { get; set; }
        public ushort PrefetchCount { get; set; }
        public ExchangeConfiguration Exchange { get; set; }
        public QueueConfiguration Queue { get; set; }
        public string RoutingKey { get; set; }

        public SubscriptionConfiguration()
        {
            Exchange = new ExchangeConfiguration();
            Queue = new QueueConfiguration();
        }
    }
}
