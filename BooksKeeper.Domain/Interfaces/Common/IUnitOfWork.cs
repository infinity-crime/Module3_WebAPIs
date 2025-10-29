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
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // если операция с БД всего одна, то лишний раз открывать не будем транзакцию
        // так как она откроется автономно при SaveChangesAsync()
        Task SaveChangesAsync();
    }
}
