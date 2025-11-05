using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs.Requests.ApplicationUserRequests;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces.Identity
{
    public interface IApplicationUserService
    {
        Task<Result<IdentityResult>> RegisterAsync(RegisterUserRequest request);

        Task<Result<string>> LoginAsync(LoginUserRequest request);
    }
}
