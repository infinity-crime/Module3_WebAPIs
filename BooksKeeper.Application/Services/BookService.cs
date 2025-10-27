using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests;
using BooksKeeper.Application.DTOs.Requests.BookRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Exceptions.BookExceptions;
using BooksKeeper.Domain.Exceptions.Common;
using BooksKeeper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<Result<BookResponse>> CreateAsync(CreateBookRequest request)
        {
            try
            {
                var newBook = Book.Create(request.Title, request.Year);

                var authors = await _authorRepository.GetByIdRangeAsync(request.AuthorIds);

                newBook.AddAuthorsRange(authors);

                await _bookRepository.AddAsync(newBook);

                return Result<BookResponse>.Success(new BookResponse(
                    newBook.Id,
                    newBook.Title,
                    newBook.Year,
                    newBook.Authors
                    .Select(a => new AuthorDto(a.Id, a.FirstName, a.LastName)).ToList()
                    ));
            }
            catch(InvalidBookAuthorException ex)
            {
                return Result<BookResponse>.Failure(Error.NotFound(ex.Code, $"One or more of the author IDs from the list are missing " +
                    $"from the database. Details: {ex.Message}"));
            }
            catch(DomainException ex)
            {
                return Result<BookResponse>.Failure(Error.Validation(ex.Code, ex.Message));
            }
            catch(Exception ex)
            {
                return Result<BookResponse>.Failure(Error.Failure("BOOK_CREATION_FAILURE", 
                    $"An error occurred while creating the book. Details: {ex.Message}"));
            }
        }

        public Task<Result<BookResponse>> CreateWithAuthorAsync(CreateBookWithAuthorRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book is null)
                return Result.Failure(Error.NotFound("BOOK_NOT_FOUND",
                    $"Book with ID '{id}' was not found."));

            try
            {            
                await _bookRepository.DeleteAsync(book);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(Error.Failure("BOOK_DELETION_FAILURE",
                    $"An error occurred while deleting the book. Details: {ex.Message}"));
            }
        }

        public async Task<IEnumerable<BookResponse>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllWithAuthorsAsync();

            return books.Select(b => MapToBookResponse(b));
        }

        public async Task<Result<BookResponse>> GetByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdWithAuthorsAsync(id);
            if(book is null)
                return Result<BookResponse>.Failure(Error.NotFound("BOOK_NOT_FOUND", 
                    $"Book with ID '{id}' was not found."));

            return Result<BookResponse>.Success(MapToBookResponse(book));
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateBookRequest request)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book is null)
                return Result.Failure(Error.NotFound("BOOK_NOT_FOUND",
                    $"Book with ID '{id}' was not found."));

            try
            {
                book.ChangeTitle(request.Title);
                book.ChangeYear(request.Year);

                var authors = await _authorRepository.GetByIdRangeAsync(request.AuthorIds);
                book.ChangeAuthorsRange(authors);

                await _bookRepository.UpdateAsync(book);

                return Result.Success();
            }
            catch (InvalidBookAuthorException ex)
            {
                return Result.Failure(Error.NotFound(ex.Code, $"One or more of the author IDs from the list are missing " +
                    $"from the database. Details: {ex.Message}"));
            }
            catch (DomainException ex)
            {
                return Result.Failure(Error.Validation(ex.Code, ex.Message));
            }
            catch (Exception ex)
            {
                return Result.Failure(Error.Failure("BOOK_UPDATE_FAILURE",
                    $"An error occurred while updating the book. Details: {ex.Message}"));
            }
        }

        private BookResponse MapToBookResponse(Book book)
        {
            return new BookResponse(
                book.Id,
                book.Title,
                book.Year,
                book.Authors
                    .Select(a => new AuthorDto(a.Id, a.FirstName, a.LastName))
                    .ToList());
        }
    }
}
