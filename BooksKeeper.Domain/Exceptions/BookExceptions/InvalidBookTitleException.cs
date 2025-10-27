using BooksKeeper.Domain.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Exceptions.BookExceptions
{
    public class InvalidBookTitleException : DomainException
    {
        public InvalidBookTitleException(string message) : base("INVALID_BOOK_TITLE", message) { }
    }
}
