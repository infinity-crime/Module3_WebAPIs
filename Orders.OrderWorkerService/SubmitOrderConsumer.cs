using MassTransit;
using Orders.Contracts.Commands;
using Orders.OrderWorkerService.Entities;
using Orders.OrderWorkerService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.OrderWorkerService
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrderCommand>
    {
        private readonly IOrderService _orderService;

        public SubmitOrderConsumer(IOrderService orderService)
        {
            _orderService = orderService;
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
        }
    }
}
