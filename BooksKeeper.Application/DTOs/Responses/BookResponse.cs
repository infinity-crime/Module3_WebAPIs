using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Responses
{
    public record BookResponse(Guid Id, string Title, int Year, List<AuthorDto> Authors);
}
