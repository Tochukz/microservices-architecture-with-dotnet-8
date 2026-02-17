using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Azure.Messaging.ServiceBus;
using NewtonSoft.Json;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        //Primary Connection String
        private string connectionString = "Endpoint=sb://mangoeb.servicebus.windows.net';.....";

        public async Task PublishMessage(object message, string topicOrQueueName)
        {
            await using ServiceBusClient client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topicOrQueueName);
            string jsonMessage = JsonConverter.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
