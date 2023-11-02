using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StockServices.API.Events;
using StockServices.API.Model;
using StockServices.API.Services;
using System.Text;

namespace StockServices.API;

public class ProductCreatedEventConsumer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProductCreatedEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void Register()
    {
        ConnectionFactory factory = new();
        factory.HostName = "localhost";
        factory.Port = 5672;
        factory.UserName = "guest";
        factory.Password = "guest";

         IConnection connection = factory.CreateConnection();
         IModel channel = connection.CreateModel();

         IStockService _stockService;

        EventingBasicConsumer consumer = new(channel);
        channel.BasicConsume(queue: nameof(ProductCreatedEvent), autoAck: false, consumer);

        consumer.Received += async (sender, e) =>
        {
            string jsonData = Encoding.UTF8.GetString(e.Body.Span);
            ProductCreatedEvent eventData = JsonConvert.DeserializeObject<ProductCreatedEvent>(jsonData);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _stockService = scope.ServiceProvider.GetService<IStockService>();
                Stock data = new()
                {
                    ProductId = eventData.ProductId,
                    Amount = 0
                };

                await _stockService.AddAysnc(data);
                await _stockService.SaveAysnc();
            }


            channel.BasicAck(deliveryTag: e.DeliveryTag, false);
        };
    }
}

