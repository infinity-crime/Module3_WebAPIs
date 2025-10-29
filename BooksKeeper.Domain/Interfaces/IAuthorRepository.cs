using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Interfaces
{
    /// <summary>
    /// Репозиторий, для работы с данными авторов
    /// </summary>
    public interface IAuthorRepository : IRepository<Author>
    {
        /// <summary>
        /// Получение автора по Id.
        /// </summary>
        /// <param name="id">Id автора (Guid)</param>
        /// <param name="includeBooks">bool переменная для включения книг в ответ</param>
        /// <param name="trackingEnable">bool переменная для отслеживания сущности на изменения (оптимизация)</param>
        /// <returns></returns>
        Task<Author?> GetByIdAsync(Guid id, bool includeBooks, bool trackingEnable);

        /// <summary>
        /// Получение всех авторов
        /// </summary>
        /// <param name="includeBooks">bool переменная для включения книг в ответ</param>
        /// <returns></returns>
        Task<IEnumerable<Author>> GetAllAsync(bool includeBooks);

        /// <summary>
        /// Получения опред. кол-ва авторов по списку с Id
        /// </summary>
        /// <param name="ids">Список Id авторов (Guid)</param>
        /// <returns></returns>
        Task<IEnumerable<Author>> GetByIdRangeAsync(List<Guid> ids);
    }
}
