using BooksKeeper.Domain.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Exceptions.AuthorExceptions
{
    public class InvalidAuthorFirstNameException : DomainException
    {
        public InvalidAuthorFirstNameException(string message) : base("INVALID_AUTHOR_FIRST-NAME", message) { }
    }
}
