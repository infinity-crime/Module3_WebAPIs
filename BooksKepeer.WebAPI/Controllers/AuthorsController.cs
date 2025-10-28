using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.AuthorRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKepeer.WebAPI.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BooksKepeer.WebAPI.Controllers
{
    /// <summary>
    /// Контроллер для управления авторами
    /// </summary>
    [Route("api/[controller]")]
    public class AuthorsController : BaseController
    {
        private readonly IAuthorService _service;

        public AuthorsController(IAuthorService service)
        {
            _service = service;
        }

        [HttpGet("all-authors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById([FromRoute] Guid id)
        {
            var author = await _service.GetByIdAsync(id);

            return HandleResult<AuthorResponse>(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorRequest request)
        {
            var newAuthor = await _service.CreateAsync(request);

            return HandleResult<AuthorDto>(newAuthor);
        }

        [HttpPut("/update/{id}")]
        public async Task<IActionResult> UpdateAuthor([FromRoute] Guid id, UpdateAuthorRequest request)
        {
            var result = await _service.UpdateAsync(id, request);

            return HandleResult(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAuthor([FromRoute] Guid id)
        {
            var result = await _service.DeleteByIdAsync(id);

            return HandleResult(result);
        }
    }
}
