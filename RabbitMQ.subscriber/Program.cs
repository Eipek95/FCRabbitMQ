using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://yunsubup:eOTyDZ80baa2afV_AR_VS-O16GCeYKlq@fish.rmq.cloudamqp.com/yunsubup");
using (var connection = factory.CreateConnection())
{
    var channel = connection.CreateModel();
    channel.BasicQos(0, 1, false);

    var consumer = new EventingBasicConsumer(channel);
    var queueName = channel.QueueDeclare().QueueName;

    Dictionary<string, object> headers = new Dictionary<string, object>();

    headers.Add("format", "pdf");
    headers.Add("shape", "a4");
    headers.Add("x-match", "any");//any olursa key-value çiftlerinin bire bile eşit olursa publisher tarafıyla sonuç alınır
                                  //headers.Add("x-match", "all");//all olursa key-value çiftlerinin hepsi eşit olmalı publisher tarafıyla

    //header-exchange -> publisher oluşturuyor eğer ki publisher çalışmadan burası çalışırsa hata verir hata vermesini istemessek her iki tarafta da oluşturabiliriz.


    channel.QueueBind(queueName, "header-exchange", string.Empty, headers);
    channel.BasicConsume(queueName, false, consumer);//subscribe-consumer düşünce kuyrukta düşssün

    Console.WriteLine("Loglar dinleniyor....");

    consumer.Received += (object sender, BasicDeliverEventArgs e) =>
    {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        Product product = JsonSerializer.Deserialize<Product>(message);
        Thread.Sleep(1500);
        Console.WriteLine("Gelen Mesaj: " + $"{product.Id + " " + product.Name + " " + product.Price + " " + product.Stock}");

        channel.BasicAck(e.DeliveryTag, false);
    };
    Console.ReadLine();

}