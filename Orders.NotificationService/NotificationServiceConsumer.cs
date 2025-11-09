using MassTransit;
using Orders.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.NotificationService
{
    public class NotificationServiceConsumer : IConsumer<OrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var order = context.Message;

            Console.WriteLine($"NotificationServiceConsumer: orderid = {order.OrderId}; createdAt {order.CreatedAt}");

            return Task.CompletedTask;
        }
    }
}
