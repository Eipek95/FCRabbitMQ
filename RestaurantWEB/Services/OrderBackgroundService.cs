
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Text;
using System.Text.Json;

namespace RestaurantWEB.Services
{
    public class OrderBackgroundService : BackgroundService
    {
        private readonly ILogger<OrderBackgroundService> _logger;
        private readonly RabbitMQService _rabbitMQService;
        private IModel _channel;

        public OrderBackgroundService(ILogger<OrderBackgroundService> logger, RabbitMQService rabbitMQService)
        {
            _logger = logger;
            _rabbitMQService = rabbitMQService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitMQService.Connect();
            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.BasicConsume(RabbitMQService.QueueName, false, consumer);


            try
            {
                consumer.Received += (sender, @event) =>
                {
                    var body = @event.Body;
                    var createOrderMessage = JsonSerializer.Deserialize<CreateOrderMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));

                    // Mesajı işle
                    _logger.LogInformation($"Order (Id:{createOrderMessage!.CustomerID}) was created by successfull");

                    // İşlemi tamamlandı olarak işaretle
                    _channel.BasicAck(deliveryTag: @event.DeliveryTag, multiple: false);

                    return Task.CompletedTask;
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
