using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs
{
    public record AuthorDto(Guid Id, string FirstName, string LastName);
}
