using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.ProductReviewRequests;
using BooksKeeper.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces
{
    public interface IProductReviewService
    {
        Task<Result<ProductReviewDto>> CreateReviewAsync(CreateReviewRequest request, CancellationToken cancellationToken = default);

        Task<Result<ProductReviewResponse>> GetReviewByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<ProductReviewDto>>> GetAllReviewsAsync(CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<ProductReviewResponse>>> GetAllByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);

        Task<Result> DeleteReviewByIdAsync(string id, CancellationToken cancellationToken);
    }
}
