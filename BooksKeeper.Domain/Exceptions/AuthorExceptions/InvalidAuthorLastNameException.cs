using BooksKeeper.Domain.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Exceptions.AuthorExceptions
{
    public class InvalidAuthorLastNameException : DomainException
    {
        public InvalidAuthorLastNameException(string message) : base("INVALID_AUTHOR_LAST-NAME", message) { }
    }
}
