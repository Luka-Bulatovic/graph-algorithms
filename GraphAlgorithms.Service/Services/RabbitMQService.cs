using GraphAlgorithms.Service.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace GraphAlgorithms.Service.Services
{


    public class RabbitMQService : IMQService, IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

        public static string RandomGraphsQueueName = "random_graphs_queue_reply";

        public RabbitMQService(string hostName)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;

            consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(consumer: consumer, queue: replyQueueName, autoAck: true);

            consumer.Received += (model, ea) =>
            {
                if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out TaskCompletionSource<string> tcs))
                    return;
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            };
        }

        public Task<string> CallAsync(string message)
        {
            var correlationId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationId, tcs);

            var props = channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: "",
                routingKey: RandomGraphsQueueName,
                basicProperties: props,
                body: messageBytes);

            return tcs.Task;
        }

        public void Dispose()
        {
            connection.Close();
            channel.Close();
        }
    }
}
