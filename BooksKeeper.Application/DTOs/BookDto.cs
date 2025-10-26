using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs
{
    public record BookDto(int Id, string Title, string Author, int Year);
}
