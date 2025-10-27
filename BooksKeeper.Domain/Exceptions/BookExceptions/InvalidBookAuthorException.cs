using BooksKeeper.Domain.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Exceptions.BookExceptions
{
    public class InvalidBookAuthorException : DomainException
    {
        public InvalidBookAuthorException(string message) : base("INVALID_BOOK_AUTHOR", message) { }
    }
}
