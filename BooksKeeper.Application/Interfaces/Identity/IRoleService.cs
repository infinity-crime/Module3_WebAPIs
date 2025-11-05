using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs.Requests.IdentityRoleRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces.Identity
{
    public interface IRoleService
    {
        Task<Result> AssignRoleAsync(AssignRoleRequest request);
    }
}
