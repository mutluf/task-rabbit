using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OrderSaga.API.Context;
using OrderSaga.API.Model;
using System.Linq.Expressions;

namespace OrderSaga.API.Services
{
    public class OrderService:IOrderService
    {
        private readonly ApplicationDbContext _context;
        
        public DbSet<Order> Table => _context.Set<Order>();


        public async Task<List<Order>> GetByIdList(List<string> userIds)
        {
            List<Order> users = await Table.AsQueryable().Where(e => userIds.Contains(e.Id.ToString())).ToListAsync();
            return users;
        }

        public async Task<bool> AddAysnc(Order Model)
        {
            EntityEntry entityEntry = await Table.AddAsync(Model);
            return entityEntry.State == EntityState.Added;
        }

        public void Delete(Order Model)
        {
            Table.Remove(Model);
        }

        public async Task<int> SaveAysnc()
        {
            return await _context.SaveChangesAsync();
        }

        public bool Update(Order Model)
        {
            EntityEntry entityEntry = _context.Update(Model);
            return entityEntry.State == EntityState.Modified;
        }

        public IQueryable<Order> GetAll()
        {
            var query = Table.AsQueryable().AsNoTracking();
            return query;
        }

        public async Task<Order?> GetByIdAysnc(string id)
        {
            var query = Table.AsQueryable().AsNoTracking();
            return await query.FirstOrDefaultAsync(data => data.Id ==Convert.ToInt32(id));
        }

        public async Task<Order?> GetSingleAysnc(Expression<Func<Order, bool>> method)
        {
            var query = await Table.AsQueryable().AsNoTracking().FirstOrDefaultAsync(method);
            return query;
        }

        public IQueryable<Order> GetWhere(Expression<Func<Order, bool>> method)
        {
            var query = Table.Where(method).AsQueryable().AsNoTracking();
            return query;
        }
    }
}
