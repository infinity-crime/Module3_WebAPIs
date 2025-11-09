using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Contracts.DTOs
{
    public record LineItemDto(Guid LineItemId, decimal Price, int Quantity);
}
