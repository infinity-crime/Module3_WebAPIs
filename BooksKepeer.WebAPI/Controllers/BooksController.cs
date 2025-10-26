using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests;
using BooksKeeper.Application.Interfaces;
using BooksKepeer.WebAPI.Common;
using BooksKepeer.WebAPI.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;

namespace BooksKepeer.WebAPI.Controllers
{
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

        [HttpGet("api-info")]
        public IActionResult GetApiInfo()
        {
            return Ok(_apiSettings);
        }

        [HttpGet("all-books")]
        public async Task<IActionResult> GetAllBooks()
        {
            return Ok(await _bookService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook([FromRoute] Guid id)
        {
            var result = await _bookService.GetBookById(id);

            return HandleResult<BookDto>(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
        {
            var result = await _bookService.CreateBook(request);

            return HandleResult<BookDto>(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] Guid id, [FromBody] UpdateBookRequest request)
        {
            var result = await _bookService.UpdateBook(id, request);

            return HandleResult(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] Guid id)
        {
            var result = await _bookService.DeleteBook(id);

            return HandleResult(result);
        }
    }
}
