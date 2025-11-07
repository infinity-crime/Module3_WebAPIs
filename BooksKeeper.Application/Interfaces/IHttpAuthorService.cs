using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.AuthorRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces
{
    public interface IHttpAuthorService
    {
        Task<AuthorResponse?> GetByIdAsync(Guid id);

        Task<IEnumerable<AuthorDto>> GetByIdRangeAsync(List<Guid> ids);

        Task<IEnumerable<AuthorResponse>> GetAllAsync();

        Task<AuthorDto?> CreateAsync(CreateAuthorRequest request);

        Task<bool> UpdateAsync(Guid id, UpdateAuthorRequest request);

        Task<bool> DeleteByIdAsync(Guid id);
    }
}
