using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Requests.ProductReviewRequests
{
    public record CreateReviewRequest(Guid BookId, string ReviewerName, int Rating, string Comment);
}
