using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<Author?> GetByIdWithBooksAsync(Guid id);

        Task<IEnumerable<Author>> GetAllWithBooksAsync();

        Task<IEnumerable<Author>> GetByIdRangeAsync(List<Guid> ids);
    }
}
