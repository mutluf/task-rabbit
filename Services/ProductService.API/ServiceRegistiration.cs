using Microsoft.EntityFrameworkCore;
using ProductService.API.Context;
using ProductService.API.RabbitMq;
using ProductService.API.Services;
using RabbitMQProducerConsumer.RabbitMq;

namespace ProductService.API
{
    public static class ServiceRegistiration
    {
        public static void ProductServiceRegistiration( this IServiceCollection services, IConfiguration config)
        {
            
            services.AddDbContext<TaskRabbitProductDbContext>(o => o.UseSqlServer(config.GetConnectionString("MicrosoftSQL")));


            services.AddScoped<IProductService, Services.ProductService>();

            services.AddScoped<IPublishService, PublishService>();
        }
    }
}
