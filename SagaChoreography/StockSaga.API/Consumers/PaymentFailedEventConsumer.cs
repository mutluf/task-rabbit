using MassTransit;
using MongoDB.Driver;
using Shared.Events;
using StockSaga.API.Model;
using StockSaga.API.Services;

namespace StockSaga.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        readonly MongoDbService _mongoDbService;
        public PaymentFailedEventConsumer(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var collection = _mongoDbService.GetCollection<Stock>();

            foreach (var item in context.Message.OrderItems)
            {
                var stock = await collection.Find(x => x.ProductId == item.ProductId).FirstOrDefaultAsync();

                Stock stock1 = await (await collection.FindAsync(s => s.ProductId == item.ProductId)).FirstOrDefaultAsync();

                if (stock != null)
                {
                    stock.Count += item.Count;
                    await collection.ReplaceOneAsync(x => x.ProductId == item.ProductId, stock);
                }


                //var collection = _mongoDbService.GetCollection<Models.Stock>();

                //foreach (var item in context.Message.OrderItems)
                //{
                //    Models.Stock stock = await (await collection.FindAsync(s => s.ProductId == item.ProductId)).FirstOrDefaultAsync();
                //    if (stock != null)
                //    {
                //        stock.Count += item.Count;
                //        await collection.FindOneAndReplaceAsync(s => s.ProductId == item.ProductId, stock);
                //    }
                //}
            }
        }
    }
}
