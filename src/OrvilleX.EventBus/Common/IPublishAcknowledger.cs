﻿using RabbitMQ.Client;
using System.Threading.Tasks;

namespace OrvilleX.EventBus.Common
{
    /// <summary>
    /// 表明Event是否需要Ack
    /// </summary>
    public interface IPublishAcknowledger
    {
        Task GetAckTask(IModel result);
    }
}
