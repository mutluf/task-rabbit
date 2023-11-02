using Newtonsoft.Json;
using ProductService.API.RabbitMq;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQProducerConsumer.RabbitMq;

public class PublishService: IPublishService
{
    public void Publish(object data, string queueName)
    {
        ConnectionFactory factory = new();
        factory.HostName = "localhost";
        factory.Port = 5672;
        factory.UserName = "guest";
        factory.Password = "guest";  

         IConnection connection = factory.CreateConnection();
         IModel channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName, exclusive: false,durable:true,autoDelete:false);

        var message = JsonConvert.SerializeObject(data);
        var body = Encoding.UTF8.GetBytes(message);
   
        channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
          
    }
}
//create-product ve rabbitmq'ye data gönderip stok init etmek.
//event fırlat- ProductCreatedEvent productId - productName
//bunu dinleyen stok-servisi veri tabanında consume ederken stok create etsin productId yerine de gelen id'yi yazsın.
/*
 *Product tablon var --> id, name
 * Stok tablon var --> id, stokYeri, productId
 * ProductCreate eden servisin var, Post ediyor ve Mongoya ekliyor.
 * Stok servisin var API, consume ediyor eventi.
 */