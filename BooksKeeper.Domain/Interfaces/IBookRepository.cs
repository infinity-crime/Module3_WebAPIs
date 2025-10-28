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

        Task<IEnumerable<BookYearCountDto>> GetBooksCountByYearAsync();
    }

    /// <summary>
    /// DTO для представления количества книг по годам (в качестве теста для задания с Dapper)
    /// </summary>
    /// <param name="Year"></param>
    /// <param name="Count"></param>
    public record BookYearCountDto(int Year, long Count);
}
