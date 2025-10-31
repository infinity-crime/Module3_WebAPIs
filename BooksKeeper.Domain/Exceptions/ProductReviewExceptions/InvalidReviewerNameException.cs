using BooksKeeper.Domain.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Exceptions.ProductReviewExceptions
{
    public class InvalidReviewerNameException : DomainException
    {
        public InvalidReviewerNameException(string message) : base("INVALID_REVIEWER_NAME", message) { }
    }
}
