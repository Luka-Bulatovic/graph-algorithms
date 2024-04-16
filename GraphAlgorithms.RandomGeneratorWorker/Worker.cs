using GraphAlgorithms.Core;
using GraphAlgorithms.Core.Factories;
using GraphAlgorithms.Core.Interfaces;
using GraphAlgorithms.Shared.DTO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using static GraphAlgorithms.Shared.Shared;

namespace GraphAlgorithms.RandomGeneratorWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IConnection _connection;
        private IModel _channel;
        public static string RandomGraphsQueueName = "random_graphs_queue";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var rabbitMQHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
            var factory = new ConnectionFactory() { HostName = rabbitMQHost, UserName = "guest", Password = "guest" };
            bool connected = false;

            while(!connected)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    connected = true;
                }
                catch(Exception ex)
                {
                    _logger.LogInformation(string.Format("Failed to connect to RabbitMQ: {0}", ex.Message));
                    Thread.Sleep(5000);
                }
            }


            _channel.QueueDeclare(queue: RandomGraphsQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(0, 1, false);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                HandleMessage(content, ea.BasicProperties, ea.DeliveryTag); // Generate graphs and send ACK/NACK
            };

            _channel.BasicConsume(RandomGraphsQueueName, false, consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private void HandleMessage(string messageContent, IBasicProperties props, ulong deliveryTag)
        {
            _logger.LogInformation($"Processing message: {messageContent}");

            try
            {
                RandomGraphRequestDTO jsonData = JsonSerializer.Deserialize<RandomGraphRequestDTO>(messageContent);
                IGraphFactory factory;

                switch ((GraphClassEnum)jsonData.GraphClassID)
                {
                    case GraphClassEnum.ConnectedGraph:
                        factory = new RandomConnectedUndirectedGraphFactory(jsonData.Data.Nodes, jsonData.Data.MinEdgesFactor);
                        break;
                    case GraphClassEnum.UnicyclicBipartiteGraph:
                        factory = new RandomUnicyclicBipartiteGraphFactory(jsonData.Data.FirstPartitionSize, jsonData.Data.SecondPartitionSize, jsonData.Data.CycleLength);
                        break;
                    case GraphClassEnum.AcyclicGraphWithFixedDiameter:
                        factory = new RandomAcyclicGraphWithFixedDiameterFactory(jsonData.Data.Nodes, jsonData.Data.Diameter);
                        break;
                    default:
                        throw new InvalidOperationException("Invalid Graph Class detected.");
                }

                List<Graph> graphs = RandomGraphsGenerator.GenerateRandomGraphsWithLargestWienerIndex(factory, jsonData.TotalNumberOfRandomGraphs, jsonData.ReturnNumberOfGraphs);
                List<string> graphsMLData = new();
                foreach (Graph graph in graphs)
                {
                    string graphML = GraphEvaluator.GetGraphMLForGraph(graph);
                    graphsMLData.Add(graphML);
                }

                // Response is a list of strings, each of them being one GraphML representation
                string responseJson = JsonSerializer.Serialize(graphsMLData);

                // Sending the response
                var responseProps = _channel.CreateBasicProperties();
                responseProps.CorrelationId = props.CorrelationId;

                _channel.BasicPublish(
                    exchange: "",
                    routingKey: props.ReplyTo,
                    basicProperties: responseProps,
                    body: Encoding.UTF8.GetBytes(responseJson));

                _channel.BasicAck(deliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error while handling response: {ex.Message}");
                _channel.BasicNack(deliveryTag, false, true); // requeue the message for retry or further handling
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("RabbitMQ connection shutdown.");
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}