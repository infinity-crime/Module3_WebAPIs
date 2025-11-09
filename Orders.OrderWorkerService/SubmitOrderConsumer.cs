using MassTransit;
using Orders.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.OrderWorkerService
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrderCommand>
    {
        public Task Consume(ConsumeContext<SubmitOrderCommand> context)
        {
            var command = context.Message;

            // logic...

            Console.WriteLine($"Processing order: {command.OrderId}");
            
            return Task.CompletedTask;
        }
    }
}
