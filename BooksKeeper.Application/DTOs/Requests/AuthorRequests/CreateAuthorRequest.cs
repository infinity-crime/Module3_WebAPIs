using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Requests.AuthorRequests
{
    public record CreateAuthorRequest(string FirstName, string LastName);
}
