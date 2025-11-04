using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Requests.IdentityRoleRequests
{
    public record AssignRoleRequest(string Email, string Role);
}
