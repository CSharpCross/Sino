namespace OrvilleX.EventBus.Configuration
{
    public interface IConsumerConfiguration
    {
        bool NoAck { get; }
        ushort PrefetchCount { get; }
        ExchangeConfiguration Exchange { get; }
        QueueConfiguration Queue { get; }
        string RoutingKey { get; }
    }
}
