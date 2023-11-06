namespace Shared.Events
{
    public class PaymentFailedEvent:IEvent
    {
        public int OrderId { get; set; }
        public string Message { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
