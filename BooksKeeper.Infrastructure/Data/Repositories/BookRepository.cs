using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Interfaces;
using BooksKeeper.Infrastructure.Data.Repositories.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Infrastructure.Data.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<Book?> GetByIdAsync(Guid id, bool includeAuthors, bool trackingEnable)
        {
            var query = _dbContext.Books.AsQueryable();

            if (!trackingEnable)
                query = query.AsNoTracking();

            if (includeAuthors)
                query = query.Include(b => b.Authors);

            return await query.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync(bool includeAuthors)
        {
            var query = _dbContext.Books
                .AsNoTracking();

            if (includeAuthors)
                query = query.Include(b => b.Authors);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<BookYearCountDto>> GetBooksCountByYearAsync()
        {
            using var connection = _dbContext.Database.GetDbConnection();
            const string sql = @"
                SELECT ""Year"", COUNT(*) AS ""Count""
                FROM ""Books""
                GROUP BY ""Year""
                ORDER BY ""Year""";

            return await connection.QueryAsync<BookYearCountDto>(sql);
        }
    }
}
