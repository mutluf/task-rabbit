using System.ComponentModel.DataAnnotations;

namespace StockServices.API.Model
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
