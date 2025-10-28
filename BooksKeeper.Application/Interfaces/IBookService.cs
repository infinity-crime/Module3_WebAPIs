using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests;
using BooksKeeper.Application.DTOs.Requests.BookRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces.Common;
using BooksKeeper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces
{
    public interface IBookService : IService<BookResponse, Guid>
    {
        /// <summary>
        /// Создание книги и добавление ее в список.
        /// </summary>
        /// <param name="request">Команда создания книги</param>
        /// <returns></returns>
        Task<Result<BookResponse>> CreateAsync(CreateBookRequest request);

        /// <summary>
        /// Обновление книги
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <param name="request">Команда обновления книги</param>
        /// <returns></returns>
        Task<Result> UpdateAsync(Guid id, UpdateBookRequest request);

        /// <summary>
        /// Создание книги вместе с автором
        /// </summary>
        /// <param name="request">Команда создания книги и автора</param>
        /// <returns></returns>
        Task<Result<BookResponse>> CreateWithAuthorAsync(CreateBookWithAuthorRequest request);

        Task<IEnumerable<BookYearCountDto>> GetCountBooksByYearAsync();
    }
}
