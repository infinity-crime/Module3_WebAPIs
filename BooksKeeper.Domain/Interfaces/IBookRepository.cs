using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByIdAsync(Guid id, bool includeAuthors, bool trackingEnable);

        Task<IEnumerable<Book>> GetAllAsync(bool includeAuthors);

        Task<IEnumerable<BookYearCountDto>> GetBooksCountByYearAsync();
    }

    /// <summary>
    /// DTO для представления количества книг по годам (в качестве теста для задания с Dapper)
    /// </summary>
    /// <param name="Year"></param>
    /// <param name="Count"></param>
    public record BookYearCountDto(int Year, long Count);
}
