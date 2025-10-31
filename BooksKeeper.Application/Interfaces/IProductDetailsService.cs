using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces
{
    public interface IProductDetailsService
    {
        Task<Result<ProductDetailsResponse>> GetBookDetailsAsync(Guid id, CancellationToken cancellationToken);
    }
}
