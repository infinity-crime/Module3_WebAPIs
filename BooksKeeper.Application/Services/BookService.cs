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
using System.Runtime.CompilerServices;
using BooksKeeper.Domain.Interfaces.Common;

namespace BooksKeeper.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookDapperRepository<BookYearCountResponse> _bookDapperRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ICacheService _cacheService;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, 
            IUnitOfWork unitOfWork, IBookDapperRepository<BookYearCountResponse> bookDapperRepository, 
            ICacheService cacheService)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
            _bookDapperRepository = bookDapperRepository;
            _cacheService = cacheService;
        }

        public async Task<Result<BookResponse>> CreateAsync(CreateBookRequest request)
        {
            try
            {
                var newBook = Book.Create(request.Title, request.Year);

                var authors = await _authorRepository.GetByIdRangeAsync(request.AuthorIds);
                var missingIds = ValidateAuthorsExists(request.AuthorIds, authors);
                if(missingIds is not null)
                    return Result<BookResponse>.Failure(Error.NotFound("AUTHORS_NOT_FOUND",
                        $"One or more authors were not found. Missing author IDs: {string.Join(',', missingIds)}"));

                newBook.AddAuthorsRange(authors);

                await _bookRepository.AddAsync(newBook);
                await _unitOfWork.SaveChangesAsync();

                return Result<BookResponse>.Success(MapToBookResponse(newBook));
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

        // Здесь используем механизм транзакций, чтобы обеспечить атомарность (так как запись сразу в 2 таблицы).
        public async Task<Result<BookResponse>> CreateWithAuthorAsync(CreateBookWithAuthorRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var newBook = Book.Create(request.Title, request.Year);
                var newAuthor = Author.Create(request.AuthorFirstName, request.AuthorLastName);
                newBook.AddAuthor(newAuthor);

                await _bookRepository.AddAsync(newBook);
                await _authorRepository.AddAsync(newAuthor);

                await _unitOfWork.CommitTransactionAsync();

                return Result<BookResponse>.Success(MapToBookResponse(newBook));
            }
            catch(InvalidBookAuthorException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                return Result<BookResponse>.Failure(Error.NotFound(ex.Code, $"One or more of the author IDs from the list are missing " +
                    $"from the database. Details: {ex.Message}"));
            }
            catch(DomainException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                return Result<BookResponse>.Failure(Error.Validation(ex.Code, ex.Message));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                return Result<BookResponse>.Failure(Error.Failure("BOOK_AUTHOR_CREATE_FAILURE",
                    $"An error occurred while creating book and author. Details: {ex.Message}"));
            }
        }

        public async Task<Result> DeleteByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id, true, true);
            if (book is null)
                return Result.Failure(Error.NotFound("BOOK_NOT_FOUND",
                    $"Book with ID '{id}' was not found."));

            try
            {
                book.DeleteAuthors();
                _bookRepository.DeleteAsync(book);

                await _unitOfWork.SaveChangesAsync();

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
            var books = await _bookRepository.GetAllAsync(true);

            return books.Select(b => MapToBookResponse(b));
        }

        public async Task<Result<BookResponse>> GetByIdAsync(Guid id)
        {
            var cachedBook = await _cacheService.GetAsync<BookResponse>($"book:{id}");
            if(cachedBook is not null)
            {
                Console.WriteLine("Книга получена из Redis");
                return Result<BookResponse>.Success(cachedBook);
            }

            var book = await _bookRepository.GetByIdAsync(id, true, false);
            if(book is null)
                return Result<BookResponse>.Failure(Error.NotFound("BOOK_NOT_FOUND", 
                    $"Book with ID '{id}' was not found."));

            await _cacheService.SetAsync($"book:{id}", MapToBookResponse(book), TimeSpan.FromMinutes(10));

            Console.WriteLine("Книга получена из базы данных");

            return Result<BookResponse>.Success(MapToBookResponse(book));
        }

        public async Task<IEnumerable<BookYearCountResponse>> GetCountBooksByYearAsync()
        {
            return await _bookDapperRepository.GetBooksByYearCountAsync();
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateBookRequest request)
        {
            var book = await _bookRepository.GetByIdAsync(id, true, true);
            if (book is null)
                return Result.Failure(Error.NotFound("BOOK_NOT_FOUND",
                    $"Book with ID '{id}' was not found."));

            try
            {
                var authors = await _authorRepository.GetByIdRangeAsync(request.AuthorIds);
                var missingIds = ValidateAuthorsExists(request.AuthorIds, authors);
                if (missingIds is not null)
                    return Result.Failure(Error.NotFound("AUTHORS_NOT_FOUND",
                        $"One or more authors were not found. Missing author IDs: {string.Join(',', missingIds)}"));

                book.ChangeAuthorsRange(authors);
                book.ChangeTitle(request.Title);
                book.ChangeYear(request.Year);

                _bookRepository.UpdateAsync(book);

                await _unitOfWork.SaveChangesAsync();

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

        /* 
         * Данный метод проверяет, все ли ID авторов нашлись из запроса или нет.
         * Если нет => возвращает список ID, которые не были найдены
         */
        private List<Guid>? ValidateAuthorsExists(List<Guid> ids, IEnumerable<Author> authors)
        {
            if(ids.Count != authors.Count())
            {
                var existingIds = authors.Select(a => a.Id).ToList();
                var missingIds = ids.Except(existingIds).ToList();

                return missingIds;
            }

            return null;
        }
    }
}
