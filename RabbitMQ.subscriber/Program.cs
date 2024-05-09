using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://yunsubup:eOTyDZ80baa2afV_AR_VS-O16GCeYKlq@fish.rmq.cloudamqp.com/yunsubup");
using (var connection = factory.CreateConnection())
{
    var channel = connection.CreateModel();
    // channel.QueueDeclare("hello-queue", true, false, false);//bu kod bu tarafta olması herhangi bir hata teşkil etmez.oladabilir olmayadabilir.publisher ve subscriber tarafında aynı kuyruk oluşuyorsa içinde geçen parametrelre aynı olmalı.publish tarafında bu kuyruk oldugun eminsek burada tekrar oluşturmaya gerek yok.


    //her bir subscribe 1 mesaj gönderir
    channel.BasicQos(0, 1, false);



    var consumer = new EventingBasicConsumer(channel);

    //channel.BasicConsume("hello-queue", true, consumer);//mesajlar doğru da işlense yanlışta işlense direk siler true olunca kuyruktan.mesaj iletildiği an kuyruktan siler
    channel.BasicConsume("hello-queue", false, consumer);//mesajları hemen silmez rabbitmq.haber bekler

    consumer.Received += (object sender, BasicDeliverEventArgs e) =>
    {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        Thread.Sleep(1500);//her bir mesaj 1.5 saniyede işlenecek gibi
        Console.WriteLine(message);

        channel.BasicAck(e.DeliveryTag, false);//rabbitmq ye bilgilerndirme yapar mesajları silmesi için
    };
    Console.ReadLine();

}