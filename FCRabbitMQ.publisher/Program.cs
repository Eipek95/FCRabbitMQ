using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://yunsubup:eOTyDZ80baa2afV_AR_VS-O16GCeYKlq@fish.rmq.cloudamqp.com/yunsubup");



using (var connection = factory.CreateConnection())
{
    var channel = connection.CreateModel();

    channel.ExchangeDeclare("logs-famout", durable: true, type: ExchangeType.Fanout);

    Enumerable.Range(1, 50).ToList().ForEach(x =>
    {
        string message = $"Log [{x}]";
        var messageBody = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("logs-famout", "", null, messageBody);
        Console.WriteLine($"Mesaj gönderilmişltir. {message} {DateTime.Now}");
    });

    Console.ReadLine();

}
