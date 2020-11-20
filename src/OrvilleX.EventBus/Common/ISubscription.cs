using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrvilleX.EventBus.Common
{
    public interface ISubscription : IDisposable
    {
        string QueueName { get; }
        string[] ConsumerTags { get; }
        bool Active { get; }
    }
}
