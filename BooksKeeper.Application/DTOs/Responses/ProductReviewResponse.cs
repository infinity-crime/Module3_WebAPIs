using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Responses
{
    public record ProductReviewResponse(string ReviewerName, int Rating, string Comment, DateTime CreatedAt);
}
