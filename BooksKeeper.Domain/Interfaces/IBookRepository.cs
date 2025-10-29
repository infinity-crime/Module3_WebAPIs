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
    /// <summary>
    /// Репозиторий, для работы с данными книг
    /// </summary>
    public interface IBookRepository : IRepository<Book>
    {
        /// <summary>
        /// Получение книги по ее Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeAuthors">bool переменная для включения авторов в ответ</param>
        /// <param name="trackingEnable">bool переменная для отслеживания сущности на изменения (оптимизация)</param>
        /// <returns></returns>
        Task<Book?> GetByIdAsync(Guid id, bool includeAuthors, bool trackingEnable);

        /// <summary>
        /// Получения всех книг
        /// </summary>
        /// <param name="includeAuthors">bool переменная для включения авторов в ответ</param>
        /// <returns></returns>
        Task<IEnumerable<Book>> GetAllAsync(bool includeAuthors);

        /// <summary>
        /// Получение результирующей выборки кол-ва книг по годам при помощи Dapper
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BookYearCountDto>> GetBooksCountByYearAsync();
    }

    /// <summary>
    /// DTO для представления количества книг по годам (в качестве теста для задания с Dapper)
    /// </summary>
    /// <param name="Year"></param>
    /// <param name="Count"></param>
    public record BookYearCountDto(int Year, long Count);
}
