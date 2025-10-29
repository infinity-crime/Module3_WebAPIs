using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Requests.BookRequests
{
    public record CreateBookRequest(string Title, int Year, List<Guid> AuthorIds);

    public record CreateBookWithAuthorRequest(string Title, int Year, string AuthorFirstName, string AuthorLastName);
}
