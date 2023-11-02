namespace ProductService.API.RabbitMq
{
    public interface IPublishService
    {
        void Publish(object data, string queueName);
    }
}
