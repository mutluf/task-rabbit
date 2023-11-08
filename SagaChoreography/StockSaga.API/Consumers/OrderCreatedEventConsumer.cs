using MassTransit;
using MongoDB.Driver;
using Shared.Events;
using StockSaga.API.Model;
using StockSaga.API.Services;

namespace StockSaga.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        readonly MongoDbService _mongoDbService;
        readonly ISendEndpointProvider _sendEndpointProvider;
        readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(
            MongoDbService mongoDbService,
            ISendEndpointProvider sendEndpointProvider,
            IPublishEndpoint publishEndpoint)
        {
            _mongoDbService = mongoDbService;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            IMongoCollection<Stock> collection = _mongoDbService.GetCollection<Stock>();

            //Sipariş edilen ürünlerin stok miktarı sipariş adedinden fazla mı? değil mi?
            foreach (OrderItemMessage orderItem in context.Message.OrderItems)
            {
                Stock stock =  collection.Find(s => s.ProductId == orderItem.ProductId ).FirstOrDefault();

                //Stock stock1 = stock.FirstOrDefault();
                
                //stockResult.Add(( collection.Find(s => s.ProductId == orderItem.ProductId && s.Count > orderItem.Count)).Any());

                stockResult.Add(stock.Count>orderItem.Count?true:false);
            }
                

            //Eğer fazlaysa sipariş edilen ürünlerin stok miktarı güncelleniyor.
            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
                foreach (OrderItemMessage orderItem in context.Message.OrderItems)
                {
                    Stock stock = await (await collection.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();
                    stock.Count -= orderItem.Count;
                    await collection.FindOneAndReplaceAsync(x => x.ProductId == orderItem.ProductId, stock);
                }

                ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{nameof(StockReservedEvent)}"));
                StockReservedEvent stockReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems,
                    TotalPrice = context.Message.TotalPrice
                };
                await sendEndpoint.Send(stockReservedEvent);
            }
            //Eğer az ise siparişin iptal edilmesi için gerekli event gönderiliyor.
            else
            {
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "Stok yetersiz..."
                };
                await _publishEndpoint.Publish(stockNotReservedEvent);
            }

            // ***
            // StockReservedEvent
            // ***
            // OR
            // ***
            // StockNotReservedEvent
            // ***
        }
    }
}
