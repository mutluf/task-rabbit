using MassTransit;
using OrderSaga.API.Model;
using OrderSaga.API.Model.Enums;
using OrderSaga.API.Services;
using Shared.Events;

namespace OrderSaga.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        readonly IOrderService _orderService;

        public PaymentFailedEventConsumer(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            string modelId = context.Message.OrderId.ToString();

            Order order = await _orderService.GetByIdAysnc(modelId);

            if (order != null)
            {
                order.OrderStatus = OrderStatus.Failed;
                _orderService.Update( order);
                await _orderService.SaveAysnc();

                Console.WriteLine(context.Message.Message);
            }
        }
    }
}
