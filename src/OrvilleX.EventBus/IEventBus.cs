using OrvilleX.EventBus.Common;
using OrvilleX.EventBus.Configuration;
using System;
using System.Threading.Tasks;

namespace OrvilleX.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T message = default(T), Action<IPublishConfigurationBuilder> configuration = null);

        ISubscription SubscribeAsync<T>(Func<T, Task> subscribeMethod, Action<ISubscriptionConfigurationBuilder> configuration = null);
    }
}
