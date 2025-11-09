using MassTransit;
using Orders.Contracts.Commands;
using Orders.Contracts.Events;
using Orders.OrderWorkerService.Entities;
using Orders.OrderWorkerService.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.OrderWorkerService
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrderCommand>
    {
        private readonly IOrderService _orderService;
        private readonly IBus _bus;

        public SubmitOrderConsumer(IOrderService orderService, IBus bus)
        {
            _orderService = orderService;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<SubmitOrderCommand> context)
        {
            var command = context.Message;
            Console.WriteLine($"Processing order: {command.OrderId}");

            var order = new Order(
                command.OrderId, 
                "TestCustomer", 
                command.LineItems
                    .Select(i => new LineItem(
                        i.LineItemId, 
                        command.OrderId, 
                        "TestProduct", 
                        i.Price, 
                        i.Quantity))
                    .ToList()
            );

            await Task.Delay(10);
            _orderService.AddOrder(order);

            await _bus.Publish(new OrderCreatedEvent(order.Id, order.CreatedAt));
        }
    }
}
