using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using StockServices.API.Model;
using StockServices.API.Context;

namespace StockServices.API.Services
{
    public class StockService : IStockService
    {
        private readonly TaskRabbitStockDbContext _context;
        public StockService(TaskRabbitStockDbContext context)
        {
            _context = context;
        }
        public DbSet<Stock> Table => _context.Set<Stock>();

        public async Task<bool> AddAysnc(Stock Model)
        {
            EntityEntry entityEntry = await Table.AddAsync(Model);
            return entityEntry.State == EntityState.Added;
        }

        public void Delete(Stock Model)
        {
            Table.Remove(Model);
        }

        public async Task<int> SaveAysnc()
        {
            return await _context.SaveChangesAsync();
        }

        public bool Update(Stock Model)
        {
            EntityEntry entityEntry = _context.Update(Model);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
