using BooksKeeper.Application.DTOs.Requests.BookRequests;
using FluentValidation;

namespace BooksKepeer.WebAPI.Validators
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
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
