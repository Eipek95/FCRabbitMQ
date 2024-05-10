using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://yunsubup:eOTyDZ80baa2afV_AR_VS-O16GCeYKlq@fish.rmq.cloudamqp.com/yunsubup");
using (var connection = factory.CreateConnection())
{
    var channel = connection.CreateModel();
    channel.BasicQos(0, 1, false);
    var consumer = new EventingBasicConsumer(channel);
    var queueName = "direct-queue-Critical"; //rabbitmq sitesinden baktım


    channel.BasicConsume(queueName, false, consumer);
    Console.WriteLine("Loglar dinleniyor....");

    consumer.Received += (object sender, BasicDeliverEventArgs e) =>
    {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        Thread.Sleep(1500);
        Console.WriteLine("Gelen Mesaj: " + message);
        File.AppendAllText("log-critical.txt", message + "\n");
        channel.BasicAck(e.DeliveryTag, false);
    };
    Console.ReadLine();

}