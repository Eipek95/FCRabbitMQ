using RabbitMQ.Client;

namespace RestaurantWEB.Services
{
    public class RabbitMQService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;


        public static string QueueName = "queue-order";
        private readonly ILogger<RabbitMQService> _logger;

        public RabbitMQService(ConnectionFactory connectionFactory, ILogger<RabbitMQService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;

        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            _channel = _connection.CreateModel();

            _logger.LogInformation("RabbitMQ ile Bağlantı Kuruldu...");

            return _channel;

        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMQ ile Bağlantı Koptu....");
        }
    }
}
