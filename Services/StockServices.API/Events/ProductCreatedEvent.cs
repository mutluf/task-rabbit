namespace StockServices.API.Events
{
    public class ProductCreatedEvent
    {
        public int ProductId { get; set; }
        public int Amount { get; set; } = 0;
    }
}
