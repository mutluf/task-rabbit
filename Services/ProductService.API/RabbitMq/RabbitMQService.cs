using RabbitMQ.Client;

namespace ProductService.API.RabbitMq
{
    public class RabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService()
        {
            ConnectionFactory factory = new();
            factory.HostName = "localhost";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public IModel GetChannel()
        {
            return _channel;
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
