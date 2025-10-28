using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Interfaces;
using BooksKeeper.Infrastructure.Data.Repositories.Common;
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

        public async Task<Book> CreateBookWithAuthorAsync(Book book, Author author)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Book>> GetAllWithAuthorsAsync()
        {
            return await _dbContext.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Book?> GetByIdForUpdateAsync(Guid id)
        {
            return await _dbContext.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync();
        }

        public async Task<Book?> GetByIdWithAuthorsAsync(Guid id)
        {
            return await _dbContext.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
