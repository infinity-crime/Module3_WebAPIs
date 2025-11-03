using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Interfaces
{
    public interface IBookDapperRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> GetBooksByYearCountAsync();
    }
}
