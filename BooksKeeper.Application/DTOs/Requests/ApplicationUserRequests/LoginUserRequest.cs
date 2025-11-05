using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Requests.ApplicationUserRequests
{
    public record LoginUserRequest(string Email, string Password);
}
