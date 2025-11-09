using MassTransit;
using Orders.Contracts.Events;

namespace Orders.InventoryService
{
    public class InventoryServiceConsumer : IConsumer<OrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var order = context.Message;

            Console.WriteLine($"InventoryServiceConsumer: orderid = {order.OrderId}; createdAt {order.CreatedAt}");

            return Task.CompletedTask;
        }
    }
}
