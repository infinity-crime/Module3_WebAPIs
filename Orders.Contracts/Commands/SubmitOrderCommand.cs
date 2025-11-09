using Orders.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Contracts.Commands
{
    public record SubmitOrderCommand(Guid OrderId, List<LineItemDto> LineItems);
}
