using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.AuthorRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces
{
    public interface IAuthorService : IService<AuthorResponse, Guid>
    {
        Task<Result<AuthorDto>> CreateAsync(CreateAuthorRequest request);

        Task<Result> UpdateAsync(Guid id, UpdateAuthorRequest request);
    }
}
