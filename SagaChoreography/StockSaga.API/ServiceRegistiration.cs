using MassTransit;
using Shared.Events;
using StockSaga.API.Consumers;
using StockSaga.API.Services;

namespace StockSaga.API
{
    public static class ServiceRegistiration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<OrderCreatedEventConsumer>();
                configure.AddConsumer<PaymentFailedEventConsumer>();

                configure.UsingRabbitMq((context, configurator) =>
                {
                    //configurator.Host(configuration.GetSection("RabbitMQSettings:Host") as RabbitMqHostSettings);
                    configurator.Host("localhost");

                    configurator.ReceiveEndpoint(nameof(OrderCreatedEvent), e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));

                    configurator.ReceiveEndpoint(nameof(PaymentFailedEvent), e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
                });
            });

            services.AddSingleton<MongoDbService>();
        }
    }
}
