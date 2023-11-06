using MassTransit;
using OrderSaga.API.Model;
using OrderSaga.API.Model.Enums;
using OrderSaga.API.Services;
using Shared.Events;

namespace OrderSaga.API.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        readonly IOrderService _orderService;

        public StockNotReservedEventConsumer(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            string id = context.Message.OrderId.ToString();

            Order order = await _orderService.GetByIdAysnc(id);
            if (order != null)
            {
                order.OrderStatus = OrderStatus.Failed;
                _orderService.Update(order);

                Console.WriteLine(context.Message.Message);
            }
        }
    }
}
