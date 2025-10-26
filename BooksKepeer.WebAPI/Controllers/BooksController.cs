using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests;
using BooksKeeper.Application.Interfaces;
using BooksKepeer.WebAPI.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace BooksKepeer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : BaseController
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("all-books")]
        public IActionResult GetAllBooks()
        {
            return Ok(_bookService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetBook([FromRoute] Guid id)
        {
            var result = _bookService.GetBookById(id);

            return HandleResult<BookDto>(result);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] CreateBookRequest request)
        {
            var result = _bookService.CreateBook(request);

            return HandleResult<BookDto>(result);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateBook([FromRoute] Guid id, [FromBody] UpdateBookRequest request)
        {
            var result = _bookService.UpdateBook(id, request);

            return HandleResult(result);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBook([FromRoute] Guid id)
        {
            var result = _bookService.DeleteBook(id);

            return HandleResult(result);
        }
    }
}
