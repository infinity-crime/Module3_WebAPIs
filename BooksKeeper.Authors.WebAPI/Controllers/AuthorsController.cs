using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.AuthorRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Authors.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksKeeper.Authors.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController : BaseController
    {
        private readonly IAuthorService _service;

        public AuthorsController(IAuthorService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получение всех авторов из БД вместе с их книгами
        /// </summary>
        /// <returns></returns>
        [HttpGet("all-authors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            return Ok(await _service.GetAllAsync());
        }

        /// <summary>
        /// Получение автора по его Id с его книгами
        /// </summary>
        /// <param name="id">Id автора (Guid)</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById([FromRoute] Guid id)
        {
            var author = await _service.GetByIdAsync(id);

            return HandleResult<AuthorResponse>(author);
        }

        /// <summary>
        /// Создание автора без добавления ему книг
        /// </summary>
        /// <param name="request">Запрос создания автора, включающий имя и фамилию</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorRequest request)
        {
            var newAuthor = await _service.CreateAsync(request);

            return HandleResult<AuthorDto>(newAuthor);
        }

        /// <summary>
        /// Обновление автора (только имя и фамилия, без добавления и удаления книг)
        /// </summary>
        /// <param name="id">Id автора (Guid)</param>
        /// <param name="request">Запрос обновления автора, включающий имя и фамилию</param>
        /// <returns></returns>
        [HttpPut("/update/{id}")]
        public async Task<IActionResult> UpdateAuthor([FromRoute] Guid id, UpdateAuthorRequest request)
        {
            var result = await _service.UpdateAsync(id, request);

            return HandleResult(result);
        }

        /// <summary>
        /// Удаление автора вместе с его зависимости (обнуляются у книг, но сами книги не удаляются).
        /// </summary>
        /// <param name="id">Id автора (Guid)</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] Guid id)
        {
            var result = await _service.DeleteByIdAsync(id);

            return HandleResult(result);
        }
    }
}
