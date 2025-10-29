using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Interfaces.Common
{
    public interface IUnitOfWork : IDisposable
    {
        // если у нас операций с БД в запросе больше 1, то будем открывать транзакцию

        /// <summary>
        /// Инициализация транзакции
        /// </summary>
        /// <returns></returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// Фиксирование изменений и подтверждение транзакции.
        /// Если использовать этот метод без BeginTransactionAsync() -> исключение!
        /// </summary>
        /// <returns></returns>
        Task CommitTransactionAsync();

        /// <summary>
        /// Откат транзакции и всех изменений в ней.
        /// Безопасен для использования в любой момент (тк проверяет на null транзакцию)
        /// </summary>
        /// <returns></returns>
        Task RollbackTransactionAsync();

        // если операция с БД всего одна, то лишний раз открывать не будем транзакцию
        // так как она откроется автономно при SaveChangesAsync()
        Task SaveChangesAsync();
    }
}
