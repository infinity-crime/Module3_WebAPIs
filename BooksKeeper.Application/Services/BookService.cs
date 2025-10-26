using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Exceptions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Services
{
    public class BookService : IBookService
    {
        private readonly List<Book> _books = new List<Book>();

        public Result<BookDto> CreateBook(CreateBookRequest request)
        {
            try
            {
                var newBook = Book.Create(_books.Count, request.Title, request.Author, request.Year);

                _books.Add(newBook);

                return Result<BookDto>.Success(MapBookToDto(newBook));
            }
            catch(DomainException ex)
            {
                return Result<BookDto>.Failure(Error.Validation(ex.Code, ex.Message));
            }
        }

        public Result DeleteBook(int id)
        {
            var bookIndex = _books.FindIndex(b => b.Id == id);
            if (bookIndex < 0)
                return Result.Failure(Error.NotFound("BOOK_ID", "Book ID not found!"));

            _books.RemoveAt(bookIndex);
            return Result.Success();
        }

        public IEnumerable<BookDto>? GetAll()
        {
            return _books.Select(b => new BookDto
            (
                b.Id,
                b.Title,
                b.Author,
                b.Year
            ));
        }

        public Result<BookDto> GetBookById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book is null)
                return Result<BookDto>.Failure(Error.NotFound("BOOK_ID", "Book ID not found!"));

            return Result<BookDto>.Success(MapBookToDto(book));
        }

        public Result UpdateBook(int id, UpdateBookRequest request)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book is null)
                return Result.Failure(Error.NotFound("BOOK_ID", "Book ID not found!"));
            
            try
            {
                book.ChangeTitle(request.Title);
                book.ChangeAuthor(request.Author);
                book.ChangeYear(request.Year);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(Error.Validation(ex.Code, ex.Message));
            }
        }

        public BookDto MapBookToDto(Book book)
        {
            return new BookDto(book.Id, book.Title, book.Author, book.Year);
        }
    }
}
