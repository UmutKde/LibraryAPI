using FluentValidation;
using LibrarySystem.Application.Dtos.CategoryDtos;

namespace LibrarySystem.Application.Validators;

public class CreateCategoryDtoValidator : AbstractValidator<CategoryDtoForInsertion>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category name is required")
            .MinimumLength(2).WithMessage("Category name must be at least 2 characters.");
    }
}