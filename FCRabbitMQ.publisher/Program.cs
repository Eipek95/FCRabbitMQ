using RabbitMQ.Client;
using System.Text;


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
            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);


            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            {
                var routeKey = $"route-{x}";
                var queueName = $"direct-queue-{x}";
                channel.QueueDeclare(queueName, true, false, false);//kuyruk ad-durable-farklı channelardan bağlanabileyim-otomatik silinmesin yani subscribe yoksa mesaj kuyrukta beklesin

                channel.QueueBind(queueName, "logs-direct", routeKey, null);
            });
            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log = (LogNames)new Random().Next(1, 5);


                string message = $"log-type: [{log}]";
                var messageBody = Encoding.UTF8.GetBytes(message);
                var routeKey = $"route-{log}";
                channel.BasicPublish("logs-direct", routeKey, null, messageBody);
                Console.WriteLine($"Log gönderilmişltir. {message} {DateTime.Now}");
            });

            Console.ReadLine();

        }
    }
}