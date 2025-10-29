using BooksKeeper.Application.DTOs.Requests.BookRequests;
using FluentValidation;

namespace BooksKepeer.WebAPI.Validators
{
    /// <summary>
    /// Валидатор для запроса обновления книги
    /// </summary>
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        /// <summary>
        /// Определение правил валидации
        /// </summary>
        public UpdateBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.");

            RuleFor(x => x.Year)
                .GreaterThan(1900)
                .WithMessage("Year must be greater than 1900.");
        }
    }
}
