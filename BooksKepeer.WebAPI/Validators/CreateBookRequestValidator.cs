using FluentValidation;
using FluentValidation.AspNetCore;
using BooksKeeper.Application.DTOs.Requests;
using BooksKeeper.Application.DTOs.Requests.BookRequests;

namespace BooksKepeer.WebAPI.Validators
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
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
