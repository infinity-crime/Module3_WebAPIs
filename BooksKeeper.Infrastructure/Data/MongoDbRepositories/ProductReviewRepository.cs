using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Interfaces;
using MongoDB.Driver;
using BooksKeeper.Application.POCO.Settings;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace BooksKeeper.Infrastructure.Data.MongoDbRepositories
{
    public class ProductReviewRepository : IReviewMongoRepository
    {
        private readonly IMongoCollection<ProductReview> _collection;
        private readonly MongoDbSettings _mongoSettings;

        public ProductReviewRepository(IMongoClient client, IOptions<MongoDbSettings> options)
        {
            _mongoSettings = options.Value;

            _collection = client.GetDatabase(_mongoSettings.DatabaseName)
                .GetCollection<ProductReview>("reviews");
        }

        public async Task AddAsync(ProductReview review, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(review, new InsertOneOptions(), cancellationToken);
        }
      
        public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(new BsonDocument())
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductReview>> GetByBookIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<ProductReview>.Filter.Eq(pr => pr.BookId, id);
            return await _collection.Find(filter)
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductReview?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<ProductReview>.Filter.Eq(pr => pr.Id, id);
            return await _collection.Find(filter)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task UpdateAsync(ProductReview review, CancellationToken cancellationToken = default)
        {
            var filter = Builders<ProductReview>.Filter.Eq(pr => pr.Id, review.Id);
            return _collection.ReplaceOneAsync(filter, review, new ReplaceOptions(), cancellationToken);
        }
    }
}
