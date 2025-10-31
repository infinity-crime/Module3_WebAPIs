using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.DTOs.Responses
{
    public record ProductDetailsResponse(BookResponse BookResponse, string avgRating, List<ProductReviewResponse> reviews);
}
