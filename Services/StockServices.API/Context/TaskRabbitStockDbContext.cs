using Microsoft.EntityFrameworkCore;
using StockServices.API.Model;

namespace StockServices.API.Context
{
    public class TaskRabbitStockDbContext : DbContext
    {
        public TaskRabbitStockDbContext(DbContextOptions<TaskRabbitStockDbContext> options) : base(options) { }

        public DbSet<Stock> Stocks { get; set; }
    }
}
