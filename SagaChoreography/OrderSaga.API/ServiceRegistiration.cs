using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderSaga.API.Consumers;
using OrderSaga.API.Context;
using OrderSaga.API.Services;
using Shared.Events;

namespace OrderSaga.API
{
    public static class ServiceRegistiration
    {      
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MicrosoftSQL")));

            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<PaymentCompletedEventConsumer>();
                configure.AddConsumer<PaymentFailedEventConsumer>();
                configure.AddConsumer<StockNotReservedEventConsumer>();

                configure.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host("localhost");

                    configurator.ReceiveEndpoint(nameof(PaymentCompletedEvent), e => e.ConfigureConsumer<PaymentCompletedEventConsumer>(context));

                    configurator.ReceiveEndpoint(nameof(PaymentFailedEvent), e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));

                    configurator.ReceiveEndpoint(nameof(StockNotReservedEvent), e => e.ConfigureConsumer<StockNotReservedEventConsumer>(context));
                });
            });
  
            services.AddScoped<IOrderService,OrderService>();
        }
    }
}
