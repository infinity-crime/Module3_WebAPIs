using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.AuthorRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Exceptions.Common;
using BooksKeeper.Domain.Interfaces;
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

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Result<AuthorDto>> CreateAsync(CreateAuthorRequest request)
        {
            try
            {
                var newAuthor = Author.Create(request.FirstName, request.LastName);

                await _authorRepository.AddAsync(newAuthor);

                return Result<AuthorDto>.Success(new AuthorDto
                (
                    newAuthor.Id,
                    newAuthor.FirstName,
                    newAuthor.LastName
                ));
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
            var author = await _authorRepository.GetByIdAsync(id);
            if (author is null)
                return Result.Failure(Error.NotFound("AUTHOR_NOT_FOUND", $"Author with ID {id} was not found."));

            try
            {
                await _authorRepository.DeleteAsync(author);

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
            var authors = await _authorRepository.GetAllAsync();

            return authors.Select(a => MapToAuthorResponse(a));
        }

        public async Task<Result<AuthorResponse>> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author is null)
                return Result<AuthorResponse>.Failure(Error.NotFound("AUTHOR_NOT_FOUND", $"Author with ID {id} was not found."));

            return Result<AuthorResponse>.Success(MapToAuthorResponse(author));
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateAuthorRequest request)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author is null)
                return Result.Failure(Error.NotFound("AUTHOR_NOT_FOUND", $"Author with ID {id} was not found."));

            try
            {
                author.ChangeFirstName(request.FirstName);
                author.ChangeLastName(request.LastName);

                await _authorRepository.UpdateAsync(author);
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
    }
}
