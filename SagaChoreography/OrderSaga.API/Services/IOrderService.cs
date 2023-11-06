using OrderSaga.API.Model;
using System.Linq.Expressions;
using static MassTransit.Logging.OperationName;

namespace OrderSaga.API.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetByIdList(List<string> userIds);
        IQueryable<Order> GetAll();
        IQueryable<Order> GetWhere(Expression<Func<Order, bool>> method);
        Task<Order> GetByIdAysnc(string id);
        Task<Order> GetSingleAysnc(Expression<Func<Order, bool>> method);
        Task<bool> AddAysnc(Order Model);
        bool Update(Order Model);
        Task<int> SaveAysnc();
        void Delete(Order Model);
    }
}
