using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderSaga.API.Model;
using OrderSaga.API.Model.Dtos;
using OrderSaga.API.Model.Enums;
using OrderSaga.API.Services;
using Shared.Events;

namespace OrderSaga.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        readonly IPublishEndpoint _publishEndpoint;
        public OrdersController(IOrderService orderService, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderDto model)
        {
            Order order = new()
            {
                BuyerId = model.BuyerId,
                OrderItems = model.OrderItems.Select(oi => new OrderItem
                {
                    Count = oi.Count,
                    Price = oi.Price,
                    ProductId = oi.ProductId
                }).ToList(),
                OrderStatus = OrderStatus.Suspended,
                TotalPrice = model.OrderItems.Sum(oi => oi.Count * oi.Price),
                CreatedDate = DateTime.Now
            };

            await _orderService.AddAysnc(order);
            await _orderService.SaveAysnc();

            OrderCreatedEvent orderCreatedEvent = new()
            {
                OrderId = order.Id,
                BuyerId = order.BuyerId,
                TotalPrice = order.TotalPrice,
                OrderItems = order.OrderItems.Select(oi => new OrderItemMessage
                {
                    Price = oi.Price,
                    Count = oi.Count,
                    ProductId = oi.ProductId
                }).ToList()
            };
            await _publishEndpoint.Publish(orderCreatedEvent);

            return Ok(true);
        }
    }
}
