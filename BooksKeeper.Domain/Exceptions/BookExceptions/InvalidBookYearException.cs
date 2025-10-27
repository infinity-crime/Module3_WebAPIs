using BooksKeeper.Domain.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Exceptions.BookExceptions
{
    public class InvalidBookYearException : DomainException
    {
        public InvalidBookYearException(string message) : base("INVALID_BOOK_YEAR", message) { }
    }
}
