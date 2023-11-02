using Microsoft.EntityFrameworkCore;
using StockServices.API.Context;
using StockServices.API.Services;

namespace StockServices.API
{
    public static class ServiceRegistiration
    {
        public static void StockServiceRegistiration( this IServiceCollection services, IConfiguration config)
        {
            
            services.AddDbContext<TaskRabbitStockDbContext>(o => o.UseSqlServer(config.GetConnectionString("MicrosoftSQL")));


            services.AddScoped<IStockService, StockService>();
            services.AddSingleton<ProductCreatedEventConsumer>();

        }
    }
}
