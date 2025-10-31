using BooksKeeper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Interfaces
{
    public interface IReviewMongoRepository
    {
        Task<ProductReview?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task<IEnumerable<ProductReview>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(ProductReview review, CancellationToken cancellationToken = default);

        Task<IEnumerable<ProductReview>> GetByBookIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
