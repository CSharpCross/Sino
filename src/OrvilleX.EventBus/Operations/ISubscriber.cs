using OrvilleX.EventBus.Common;
using OrvilleX.EventBus.Configuration;
using System;
using System.Threading.Tasks;

namespace OrvilleX.EventBus.Operations
{
    public interface ISubscriber
    {
        ISubscription SubscribeAsync<T>(Func<T, Task> subscribeMethod, SubscriptionConfiguration config);
    }
}
