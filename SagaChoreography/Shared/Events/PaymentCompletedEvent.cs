namespace Shared.Events
{
    public class PaymentCompletedEvent: IEvent
    {
        public int OrderId { get; set; }
    }
}
