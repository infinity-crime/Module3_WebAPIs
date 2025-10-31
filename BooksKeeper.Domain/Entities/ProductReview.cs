using BooksKeeper.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using BooksKeeper.Domain.Exceptions.ProductReviewExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Entities
{
    public sealed class ProductReview : BaseMongoEntity<string>
    {
        [BsonRepresentation(BsonType.String)]
        public Guid BookId { get; private set; }

        [BsonElement("reviewerName")]
        public string ReviewerName { get; private set; } = string.Empty;

        [BsonElement("rating")]
        public int Rating { get; private set; }

        [BsonElement("comment")]
        public string Comment { get; private set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; private set; }

        [BsonConstructor]
        private ProductReview() { }

        public ProductReview Create(Guid bookId, string reviewerName, int rating, string comment)
        {
            if(string.IsNullOrWhiteSpace(reviewerName))
                throw new InvalidReviewerNameException("Reviewer name cannot be null or empty.");

            if(rating < 0 || rating > 5)
                throw new NotSupportedReviewRatingException("Rating must be between 0 and 5.");

            if (string.IsNullOrEmpty(comment))
                throw new InvalidReviewCommentException("Comment cannot be null or empty.");

            return new ProductReview
            {
                Id = Guid.NewGuid().ToString(),
                BookId = bookId,
                ReviewerName = reviewerName,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
