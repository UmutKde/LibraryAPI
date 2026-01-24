using FluentValidation;
using LibrarySystem.Application.Dtos.BookDtos;

namespace LibrarySystem.Application.Validators;

public class UpdateBookDtoValidator : AbstractValidator<BookDtoForUpdate>
{
    public UpdateBookDtoValidator()
    {
        RuleFor(x => x.BookName)
            .NotEmpty().WithMessage("Book name is required.")
            .MinimumLength(2).WithMessage("Book name must be at least 2 characters.");

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN number is required.")
            .Length(13).WithMessage("ISBN number must be at 13 characters.");

        RuleFor(x => x.PageCount)
            .NotEmpty().WithMessage("Page count is required.")
            .GreaterThan(0).WithMessage("Page count must be greater than 0.");
        
        RuleFor(x => x.AuthorIds)
            .NotEmpty().WithMessage("At least one author is required.");
        
        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage("At least one category is required.");
        
        RuleFor(x => x.PublisherId)
            .NotEmpty().WithMessage("At least one publisher is required.");
    }
}