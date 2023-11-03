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
