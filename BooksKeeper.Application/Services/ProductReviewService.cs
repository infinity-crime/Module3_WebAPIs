using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.ProductReviewRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Exceptions.Common;
using BooksKeeper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IReviewMongoRepository _reviewRepository;

        public ProductReviewService(IReviewMongoRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Result<ProductReviewDto>> CreateReviewAsync(CreateReviewRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var newReview = ProductReview.Create(request.BookId, request.ReviewerName, 
                    request.Rating, request.Comment);

                await _reviewRepository.AddAsync(newReview, cancellationToken);

                return Result<ProductReviewDto>.Success(MapToDto(newReview));
            }
            catch(DomainException ex)
            {
                return Result<ProductReviewDto>.Failure(Error.Validation(ex.Code, ex.Message));
            }
            catch(Exception ex)
            {
                return Result<ProductReviewDto>.Failure(Error.Failure("product_review_creation_failed", 
                    $"An unexpected error occurred while creating the product review: {ex.Message}"));
            }
        }

        public async Task<Result> DeleteReviewByIdAsync(string id, CancellationToken cancellationToken)
        {
            var review = _reviewRepository.GetByIdAsync(id, cancellationToken);
            if(review is null)
                return Result.Failure(Error.NotFound("product_review_not_found", 
                    $"Product review with id {id} was not found."));

            try
            {
                await _reviewRepository.DeleteByIdAsync(id, cancellationToken);

                return Result.Success();
            }
            catch(Exception ex)
            {
                return Result.Failure(Error.Failure("product_review_deletion_failed", 
                    $"An unexpected error occurred while deleting the product review: {ex.Message}"));
            }
        }

        public async Task<Result<IEnumerable<ProductReviewResponse>>> GetAllByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default)
        {
            var reviews = await _reviewRepository.GetByBookIdAsync(bookId, cancellationToken);

            return Result<IEnumerable<ProductReviewResponse>>.Success(
                reviews.Select(r => MapToResponse(r))
                );
        }

        public async Task<Result<IEnumerable<ProductReviewDto>>> GetAllReviewsAsync(CancellationToken cancellationToken = default)
        {
            var reviews = await _reviewRepository.GetAllAsync(cancellationToken);

            return Result<IEnumerable<ProductReviewDto>>.Success(
                reviews.Select(r => MapToDto(r))
                );
        }

        public async Task<Result<ProductReviewResponse>> GetReviewByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var review = await _reviewRepository.GetByIdAsync(id, cancellationToken);
            if(review is null)
                return Result<ProductReviewResponse>.Failure(Error.NotFound("product_review_not_found", 
                    $"Product review with id {id} was not found."));

            return Result<ProductReviewResponse>.Success(MapToResponse(review));
        }

        private ProductReviewDto MapToDto(ProductReview productReview)
        {
            return new ProductReviewDto(
                productReview.Id, 
                productReview.BookId, 
                productReview.ReviewerName,
                productReview.Rating, 
                productReview.Comment,
                productReview.CreatedAt
                );
        }

        private ProductReviewResponse MapToResponse(ProductReview productReview)
        {
            return new ProductReviewResponse(
                productReview.ReviewerName,
                productReview.Rating,
                productReview.Comment,
                productReview.CreatedAt
                );
        }
    }
}
