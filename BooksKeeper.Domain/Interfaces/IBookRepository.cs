using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByIdWithAuthorsAsync(Guid id);

        Task<IEnumerable<Book>> GetAllWithAuthorsAsync();

        Task<Book> CreateBookWithAuthorAsync(Book book, Author author);
    }
}
