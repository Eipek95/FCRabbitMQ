using RabbitMQ.API.Entities;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.API.Services
{
    public class RabbitMQService
    {
        private readonly RabbitMQClientService _rabbitmqClientService;

        public RabbitMQService(RabbitMQClientService rabbitmqClientService)
        {
            _rabbitmqClientService = rabbitmqClientService;
        }

        public void Publish(Order order)
        {
            _rabbitmqClientService.Connect();

            var bodyString = JsonSerializer.Serialize(order);

            var bodyByte = Encoding.UTF8.GetBytes(bodyString);



        }
    }
}
