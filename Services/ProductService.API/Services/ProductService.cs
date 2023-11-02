using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductService.API.Context;
using ProductService.API.Model;

namespace ProductService.API.Services
{
    public class ProductService: IProductService
    {
        private readonly TaskRabbitProductDbContext _context;

        public ProductService(TaskRabbitProductDbContext context)
        {
            _context = context;
        }

        public DbSet<Product> Table => _context.Set<Product>();

        public async Task<bool> AddAysnc(Product Model)
        {
            EntityEntry entityEntry = await _context.Products.AddAsync(Model);

            return entityEntry.State == EntityState.Added;
        }


        public void Delete(Product Model)
        {
            _context.Remove(Model);
        }

        public IQueryable<Product> GetAll()
        {
            IQueryable<Product> datas = Table.AsQueryable();
            return datas;
        }

        public async Task<Product> GetByIdAysnc(string id)
        {
            var data = await _context.Products.FindAsync(id);
            return data;
        }

        public async Task<int> SaveAysnc()
        {
            int id = await _context.SaveChangesAsync();
            return id;
        }

        public bool Update(Product Model)
        {
           var data =  _context.Update(Model);

            if(data == null)
            {
                return false;
            }

            return true;
        }
    }
}
