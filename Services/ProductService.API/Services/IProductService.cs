using ProductService.API.Model;
using System.Linq.Expressions;

namespace ProductService.API.Services
{
    public interface IProductService
    {
        IQueryable<Product> GetAll();
        Task<Product> GetByIdAysnc(int id);
        Task<bool> AddAysnc(Product Model);
        bool Update(Product Model);
        Task<int> SaveAysnc();
        void Delete(Product Model);
    }
}
