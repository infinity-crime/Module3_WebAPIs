using BooksKeeper.Domain.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Exceptions.ProductReviewExceptions
{
    public class NotSupportedReviewRatingException : DomainException
    {
        public NotSupportedReviewRatingException(string message) : base("NOT_SUPPORT_REVIEW_RATING", message) { }
    }
}
