using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces
{
    public interface IBookService
    {
        /// <summary>
        /// Получение всего списка книг, которые хранятся в памяти
        /// </summary>
        /// <returns>Список книг</returns>
        IEnumerable<BookDto>? GetAll();

        /// <summary>
        /// Получение книги по ее ID
        /// </summary>
        /// <param name="id">ID книги (int)</param>
        /// <returns></returns>
        Result<BookDto> GetBookById(int id);

        /// <summary>
        /// Создание книги и добавление ее в список.
        /// </summary>
        /// <param name="request">Команда создания книги</param>
        /// <returns></returns>
        Result<BookDto> CreateBook(CreateBookRequest request);

        /// <summary>
        /// Обновление книги
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <param name="request">Команда обновления книги</param>
        /// <returns></returns>
        Result UpdateBook(int id, UpdateBookRequest request);

        /// <summary>
        /// Удаление выбранной книги
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result DeleteBook(int id);
    }
}
