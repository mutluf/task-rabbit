using StockServices.API.Model;

namespace StockServices.API.Services
{
    public interface IStockService
    {
        Task<bool> AddAysnc(Stock Model);
        bool Update(Stock Model);
        Task<int> SaveAysnc();
        void Delete(Stock Model);
    }
}
