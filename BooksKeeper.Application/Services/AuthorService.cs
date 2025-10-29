using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.AuthorRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Exceptions.Common;
using BooksKeeper.Domain.Interfaces;
using BooksKeeper.Domain.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AuthorDto>> CreateAsync(CreateAuthorRequest request)
        {
            try
            {
                var newAuthor = Author.Create(request.FirstName, request.LastName);

                await _authorRepository.AddAsync(newAuthor);

                await _unitOfWork.SaveChangesAsync();

                return Result<AuthorDto>.Success(MapToDto(newAuthor));
            }
            catch(DomainException ex)
            {
                return Result<AuthorDto>.Failure(Error.Validation(ex.Code, ex.Message));
            }
            catch (Exception ex)
            {
                return Result<AuthorDto>.Failure(Error.Failure("AUTHOR_CREATION_FAILURE",
                    $"An error occurred while creating the author. Details: {ex.Message}"));
            }
        }

        public async Task<Result> DeleteByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id, true, true);
            if (author is null)
                return Result.Failure(Error.NotFound("AUTHOR_NOT_FOUND", $"Author with ID {id} was not found."));

            try
            {
                author.DeleteAllBooks();
                _authorRepository.DeleteAsync(author);

                await _unitOfWork.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(Error.Failure("AUTHOR_DELETION_FAILURE",
                    $"An error occurred while deleting the author. Details: {ex.Message}"));
            }
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllAsync()
        {
            var authors = await _authorRepository.GetAllAsync(true);

            return authors.Select(a => MapToAuthorResponse(a));
        }

        public async Task<Result<AuthorResponse>> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id, true, false);
            if (author is null)
                return Result<AuthorResponse>.Failure(Error.NotFound("AUTHOR_NOT_FOUND", $"Author with ID {id} was not found."));

            return Result<AuthorResponse>.Success(MapToAuthorResponse(author));
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateAuthorRequest request)
        {
            var author = await _authorRepository.GetByIdAsync(id, false, true);
            if (author is null)
                return Result.Failure(Error.NotFound("AUTHOR_NOT_FOUND", $"Author with ID {id} was not found."));

            try
            {
                author.ChangeFirstName(request.FirstName);
                author.ChangeLastName(request.LastName);

                _authorRepository.UpdateAsync(author);

                await _unitOfWork.SaveChangesAsync();

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(Error.Validation(ex.Code, ex.Message));
            }
            catch (Exception ex)
            {
                return Result.Failure(Error.Failure("AUTHOR_UPDATE_FAILURE",
                    $"An error occurred while updating the author. Details: {ex.Message}"));
            }
        }

        private AuthorResponse MapToAuthorResponse(Author author)
        {
            return new AuthorResponse
            (
                author.Id,
                author.FirstName,
                author.LastName,
                author.Books
                .Select(b => new BookDto(b.Id, b.Title, b.Year))
                .ToList()
            );
        }

        private AuthorDto MapToDto(Author author)
        {
            return new AuthorDto(author.Id, author.FirstName, author.LastName);
        }
    }
}
