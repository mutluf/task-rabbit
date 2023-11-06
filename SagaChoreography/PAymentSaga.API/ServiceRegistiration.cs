using MassTransit;
using PaymentSaga.API.Consumers;
using Shared.Events;

namespace PaymentSaga.API
{
    public static class ServiceRegistiration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<StockReservedEventConsumer>();

                configure.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host("localhost");
                    configurator.ReceiveEndpoint(nameof(StockReservedEvent), e => e.ConfigureConsumer<StockReservedEventConsumer>(context));
                });
            });

        }
    }
}
