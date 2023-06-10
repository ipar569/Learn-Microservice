using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;

        private readonly IConnection _connection;
        
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;
            var factory = new ConnectionFactory() 
                {
                    HostName = _config["RabbitMQHost"],
                    Port = //AmqpTcpEndpoint.UseDefaultPort
                    int.Parse(_config["RabbitMQPort"]),
                };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine($"--> Connected to Message Bus");
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not connect to the message bus: {e.Message}");
            }
        }

        void IMessageBusClient.PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            string message = JsonSerializer.Serialize(platformPublishedDto);

            if(_connection.IsOpen)
            {
                Console.WriteLine($"--> RabbitMQ Connection Open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine($"--> RabbitMQ Connection is closed, not sending");
            }
        }

        private void SendMessage(string message)
        {
            byte[] body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);

            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine($"--> Message But Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs args)
        {
            Console.WriteLine($"--> RabbitMQ Connection Shutdown");
        }
    }
}