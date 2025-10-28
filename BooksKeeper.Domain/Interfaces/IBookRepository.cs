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
        Task<Book?> GetByIdForUpdateAsync(Guid id);

        Task<Book?> GetByIdWithAuthorsAsync(Guid id);

        Task<IEnumerable<Book>> GetAllWithAuthorsAsync();

        // сделаем метод для добавления книги без SaveChangesAsync()
        // чтобы использовать в транзакции (паттерн единица работы добавлять не будем)
        Task AddWithoutSaveAsync(Book book);
    }
}
