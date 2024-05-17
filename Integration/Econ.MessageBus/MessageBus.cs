
using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Econ.MessageBus;

public class MessageBus : IMessageBus
{
  private readonly string connectionString = "Endpoint=sb://econweb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aFU8Y+OrXdck/xFmWXDqHR/b0xAnzIoZd+ASbKi9Xxk=";
  public async Task PublishMessage(object message, string topic_queue_Name)
  {
    await using var client = new ServiceBusClient(connectionString);

    ServiceBusSender sender = client.CreateSender(topic_queue_Name);

    var jsonMessage = JsonConvert.SerializeObject(message);
    ServiceBusMessage finalMessage = new(Encoding.UTF8.GetBytes(jsonMessage))
    {
      CorrelationId = Guid.NewGuid().ToString(),
    };

    await sender.SendMessageAsync(finalMessage);
    await client.DisposeAsync();
  }
}
