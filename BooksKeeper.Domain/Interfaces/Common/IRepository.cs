using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Interfaces.Common
{
    /// <summary>
    /// Обобщенный репозиторий, который содержит базовые одинаковые методы для остальных репозиториев.
    /// Примечание: методы не используют SavaChangesAsync()! Этим занимается UoW.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Добавление сущности в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Обновление состояние сущности
        /// </summary>
        /// <param name="entity"></param>
        void UpdateAsync(T entity);

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Проверка наличия сущности
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid id);
    }
}
