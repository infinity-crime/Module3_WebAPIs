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
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Author>> GetAllWithBooksAsync()
        {
            return await _dbContext.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetByIdRangeAsync(List<Guid> ids)
        {
            return await _dbContext.Authors
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
        }

        public async Task<Author?> GetByIdWithBooksAsync(Guid id)
        {
            return await _dbContext.Authors
                .Include(a => a.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
