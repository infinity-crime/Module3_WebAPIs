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

        public async Task<Author?> GetByIdAsync(Guid id, bool includeBooks, bool trackingEnable)
        {
            var query = _dbContext.Authors.AsQueryable();

            if (!trackingEnable)
                query = query.AsNoTracking();

            if (includeBooks)
                query = query.Include(a => a.Books);

            return await query.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Author>> GetAllAsync(bool includeBooks)
        {
            var query = _dbContext.Authors
                .AsNoTracking();

            if (includeBooks)
                query = query.Include(a => a.Books);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetByIdRangeAsync(List<Guid> ids)
        {
            return await _dbContext.Authors
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
        }
    }
}
