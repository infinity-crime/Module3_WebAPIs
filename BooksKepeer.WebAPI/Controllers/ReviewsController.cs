using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.ProductReviewRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKepeer.WebAPI.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksKepeer.WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class ReviewsController : BaseController
    {
        private readonly IProductReviewService _reviewService;

        public ReviewsController(IProductReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// Получение отзыва по его Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewsById(string id, CancellationToken cancellationToken)
        {
            var result = await _reviewService.GetReviewByIdAsync(id, cancellationToken);

            return HandleResult<ProductReviewResponse>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("all-reviews")]
        public async Task<IActionResult> GetAllReviews(CancellationToken cancellationToken)
        {
            var result = await _reviewService.GetAllReviewsAsync(cancellationToken);

            return HandleResult<IEnumerable<ProductReviewDto>>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("book/{id}")]
        public async Task<IActionResult> GetReviewsByBookId(Guid id, CancellationToken cancellationToken)
        {
            var result = await _reviewService.GetAllByBookIdAsync(id, cancellationToken);

            return HandleResult<IEnumerable<ProductReviewResponse>>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createReviewDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest createReviewDto, CancellationToken cancellationToken)
        {
            var result = await _reviewService.CreateReviewAsync(createReviewDto, cancellationToken);

            return HandleResult<ProductReviewDto>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReviewById(string id, CancellationToken cancellationToken)
        {
            var result = await _reviewService.DeleteReviewByIdAsync(id, cancellationToken);

            return HandleResult(result);
        }
    }
}
