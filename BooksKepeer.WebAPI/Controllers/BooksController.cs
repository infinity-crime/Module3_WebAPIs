using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests;
using BooksKeeper.Application.DTOs.Requests.BookRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKepeer.WebAPI.Common;
using BooksKepeer.WebAPI.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;

namespace BooksKepeer.WebAPI.Controllers
{
    /// <summary>
    /// API для управления списком книг
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : BaseController
    {
        private readonly IBookService _bookService;
        private readonly ApiSettings _apiSettings;

        public BooksController(IBookService bookService, IOptions<ApiSettings> apiOptions)
        {
            _bookService = bookService;
            _apiSettings = apiOptions.Value;
        }

        /// <summary>
        /// Получение текущих настроек API
        /// </summary>
        /// <returns>Объект, хранящий настройки</returns>
        [HttpGet("api-info")]
        public IActionResult GetApiInfo()
        {
            return Ok(_apiSettings);
        }

        /// <summary>
        /// Получение списка книг
        /// </summary>
        /// <returns>Список книг</returns>
        [HttpGet("all-books")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _bookService.GetAllAsync());
        }

        /// <summary>
        /// Получение выбранной книги по ее ID
        /// </summary>
        /// <param name="id">ID книги типа Guid</param>
        /// <returns>DTO выбранной книги</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook([FromRoute] Guid id)
        {
            var result = await _bookService.GetByIdAsync(id);

            return HandleResult<BookResponse>(result);
        }

        /// <summary>
        /// Метод, использующий Dapper для получения количества книг по годам.
        /// </summary>
        /// <returns></returns>
        [HttpGet("count-books-by-year")]
        public async Task<IActionResult> GetCountBooksByYear()
        {
            var result = await _bookService.GetCountBooksByYearAsync();
            return Ok(result);
        }

        /// <summary>
        /// Создание книги
        /// </summary>
        /// <param name="request">Запрос (DTO) с данными для создания</param>
        /// <returns>Созданная книга</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
        {
            var result = await _bookService.CreateAsync(request);

            return HandleResult<BookResponse>(result);
        }

        /// <summary>
        /// Создание книги и автора одновременно
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("/create-book-and-author")]
        public async Task<IActionResult> CreateBookWithAuthor([FromBody] CreateBookWithAuthorRequest request)
        {
            var result = await _bookService.CreateWithAuthorAsync(request);

            return HandleResult<BookResponse>(result);
        }

        /// <summary>
        /// Обновление выбранной книги по ее ID
        /// </summary>
        /// <param name="id">ID книги типа Guid</param>
        /// <param name="request">Запрос (DTO) с данными для обновления</param>
        /// <returns>Status code</returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] Guid id, [FromBody] UpdateBookRequest request)
        {
            var result = await _bookService.UpdateAsync(id, request);

            return HandleResult(result);
        }

        /// <summary>
        /// Удаление выбранной книги по ее ID
        /// </summary>
        /// <param name="id">ID книги типа Guid</param>
        /// <returns>Status code</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] Guid id)
        {
            var result = await _bookService.DeleteByIdAsync(id);

            return HandleResult(result);
        }
    }
}
