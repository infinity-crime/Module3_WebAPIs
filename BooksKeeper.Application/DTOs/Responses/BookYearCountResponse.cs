using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Responses
{
    public record BookYearCountResponse(int Year, long Count);
}
