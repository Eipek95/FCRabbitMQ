using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://yunsubup:eOTyDZ80baa2afV_AR_VS-O16GCeYKlq@fish.rmq.cloudamqp.com/yunsubup");
using (var connection = factory.CreateConnection())
{
    var channel = connection.CreateModel();



    //var randomQueueName = "log-database-save-queue";//kuyruk kalıcı hale gelmesi için random isim vermedik

    var randomQueueName = channel.QueueDeclare().QueueName;
    channel.QueueDeclare(randomQueueName, true, false, false);//biz burada queue declare etmiyoruz.subscribe etmeyi durdursa bile kuyruk durur bizim senaryomuz ilgili kuyruk silinsin

    channel.QueueBind(randomQueueName, "logs-famout", "", null);
    channel.BasicQos(0, 1, false);


    var consumer = new EventingBasicConsumer(channel);

    channel.BasicConsume(randomQueueName, false, consumer);

    Console.WriteLine("Loglar dinleniyor.....");

    consumer.Received += (object sender, BasicDeliverEventArgs e) =>
    {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        Thread.Sleep(1500);//her bir mesaj 1.5 saniyede işlenecek gibi
        Console.WriteLine(message);

        channel.BasicAck(e.DeliveryTag, false);
    };
    Console.ReadLine();

}