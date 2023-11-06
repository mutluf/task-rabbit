using OrderSaga.API.Model.Enums;

namespace OrderSaga.API.Model.Dtos
{
    public class OrderDto
    {
        public int BuyerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    
    }
}
