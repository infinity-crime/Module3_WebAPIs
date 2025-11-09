using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Contracts.Events
{
    public record OrderCreatedEvent(Guid OrderId, DateTime CreatedAt);
}
