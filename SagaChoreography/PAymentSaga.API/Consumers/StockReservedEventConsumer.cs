﻿using MassTransit;
using Shared.Events;

namespace PaymentSaga.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            // Process ...
            // Process ...
            // Process ...
            bool paymentState = true;

            //Ödeme başarılıysa
            if (paymentState)
            {
                Console.WriteLine("Ödeme başarılı...");
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };
                await _publishEndpoint.Publish(paymentCompletedEvent);
            }
            else
            {
                Console.WriteLine("Ödeme başarısız...");
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems,
                    Message = "Bakiye yetersiz!"
                };
                await _publishEndpoint.Publish(paymentFailedEvent);
            }
        }
    }
}
