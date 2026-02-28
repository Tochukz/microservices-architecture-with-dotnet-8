using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
      
        public async Task PublishMessage(object message, string topicOrQueueName)
        {
            string? connectionString = Environment.GetEnvironmentVariable("SERVICE_BUS_CONNECTION_STRING");
            await using ServiceBusClient client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topicOrQueueName);
            string jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
