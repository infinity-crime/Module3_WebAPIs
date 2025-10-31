using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Services
{
    public class ProductDetailsService : IProductDetailsService
    {
        private readonly IBookService _bookService;
        private readonly IProductReviewService _productReviewService;

        public ProductDetailsService(IBookService bookService, IProductReviewService productReviewService)
        {
            _bookService = bookService;
            _productReviewService = productReviewService;
        }

        public async Task<Result<ProductDetailsResponse>> GetBookDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            var bookResult = await _bookService.GetByIdAsync(id);
            if(!bookResult.IsSuccess)
                return Result<ProductDetailsResponse>.Failure(bookResult.Error!);

            var reviewsResult = await _productReviewService.GetAllByBookIdAsync(id, cancellationToken);
            if(!reviewsResult.IsSuccess)
                return Result<ProductDetailsResponse>.Failure(reviewsResult.Error!);

            var productDetails = new ProductDetailsResponse(
                BookResponse: bookResult.Value,
                reviews: reviewsResult.Value.ToList()
                );

            return Result<ProductDetailsResponse>.Success(productDetails);
        }
    }
}
