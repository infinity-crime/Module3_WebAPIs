using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Requests
{
    public record UpdateBookRequest(string Title, string Author, int Year);
}
