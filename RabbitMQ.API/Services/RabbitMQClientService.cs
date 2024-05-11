using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class RabbitMQClientService : IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private readonly ILogger<RabbitMQClientService> _logger;

    public static string ExchangeName = "ordersExchange";
    public static string RoutingKey = "orderQueue";
    public static string QueueName = "orderQueue";

    public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public void Connect()
    {
        _connection = _connectionFactory.CreateConnection();

        if (_channel is { IsOpen: true })
        {
            return;
        }

        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(ExchangeName, type: "direct", durable: true, autoDelete: false);
        _channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(QueueName, ExchangeName, RoutingKey);
        _logger.LogInformation("RabbitMQ ile bağlantı kuruldu...");
    }

    public void StartListening(Action<string> handleMessage)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            handleMessage(message);
        };
        _channel.BasicConsume(queue: QueueName,
                              autoAck: true,
                              consumer: consumer);
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        _channel?.Dispose();
        _connection?.Dispose();
        _logger.LogInformation("RabbitMQ ile bağlantı koptu...");
    }
}
