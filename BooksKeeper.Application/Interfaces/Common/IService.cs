using BooksKeeper.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces.Common
{
    public interface IService<T_Entity, T_Id> 
        where T_Entity : class 
        where T_Id : struct
    {
        Task<IEnumerable<T_Entity>> GetAllAsync();

        Task<Result<T_Entity>> GetByIdAsync(T_Id id);

        Task<Result> DeleteByIdAsync(T_Id id);
    }
}
