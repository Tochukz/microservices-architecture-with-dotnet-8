using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(object message, string topicOrQueueName);
    }
}
