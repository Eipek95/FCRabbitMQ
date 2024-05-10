using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;


public enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Info = 4
}
public class Program
{
    private static void Main(string[] args)
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqps://yunsubup:eOTyDZ80baa2afV_AR_VS-O16GCeYKlq@fish.rmq.cloudamqp.com/yunsubup");



        using (var connection = factory.CreateConnection())
        {
            var channel = connection.CreateModel();
            channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

            Dictionary<string, object> headers = new Dictionary<string, object>();

            headers.Add("format", "pdf");
            headers.Add("shape2", "a4");

            var properties = channel.CreateBasicProperties();
            properties.Headers = headers;
            properties.Persistent = true;//mesajları kalıcı hale getirir.rabbitmq restart olsa bile mesajlar silinmeyecek


            var product = new Product
            {
                Id = 1,
                Name = "Kalem",
                Price = 100,
                Stock = 10
            };

            var productJsonString = JsonSerializer.Serialize<Product>(product);
            channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes(productJsonString));

            Console.WriteLine("mesaj gönderilmiştir!");
            Console.ReadLine();

        }
    }
}