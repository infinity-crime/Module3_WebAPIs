using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Interfaces.Common
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);

        void UpdateAsync(T entity);

        void DeleteAsync(T entity);

        Task<bool> ExistsAsync(Guid id);
    }
}
