using Microsoft.EntityFrameworkCore;
using ProductService.API.Model;

namespace ProductService.API.Context
{
    public class TaskRabbitProductDbContext : DbContext
    {
        public TaskRabbitProductDbContext(DbContextOptions<TaskRabbitProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
