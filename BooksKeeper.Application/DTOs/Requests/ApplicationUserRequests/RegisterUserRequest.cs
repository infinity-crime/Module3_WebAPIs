using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Requests.ApplicationUserRequests
{
    public record RegisterUserRequest(string Email, string Password, DateOnly DateOfBirth);
}
