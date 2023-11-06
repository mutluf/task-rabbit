using MassTransit;
using OrderSaga.API.Model;
using OrderSaga.API.Model.Enums;
using OrderSaga.API.Services;
using Shared.Events;

namespace OrderSaga.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly IOrderService _orderService;

        public PaymentCompletedEventConsumer(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            Order order = await _orderService.GetByIdAysnc(context.Message.OrderId.ToString());

            if (order != null)
            {
                order.OrderStatus = OrderStatus.Completed;
                _orderService.Update(order);
            }
        }
    }
}
