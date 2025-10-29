using BooksKeeper.Application.DTOs.Requests.AuthorRequests;
using FluentValidation;

namespace BooksKepeer.WebAPI.Validators
{
    /// <summary>
    /// Валидатор для запроса обновления автора
    /// </summary>
    public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
    {
        /// <summary>
        /// Определение правил валидации
        /// </summary>
        public UpdateAuthorRequestValidator()
        {
            RuleFor(a => a.FirstName)
                .NotEmpty()
                .WithMessage("The author's first name must not be empty.")
                .MaximumLength(100)
                .WithMessage("The author's first name must not be longer than 100 characters.");

            RuleFor(a => a.LastName)
                .NotEmpty()
                .WithMessage("The author's second name must not be empty.")
                .MaximumLength(100)
                .WithMessage("The author's second name must not be longer than 100 characters.");
        }
    }
}
