using FluentValidation;
using LibrarySystem.Application.Dtos;
using LibrarySystem.Domain.Constants;

namespace LibrarySystem.Application.Validators;

public class UpdateAuthorDtoValidator : AbstractValidator<AuthorDtoForUpdate>
{
    public UpdateAuthorDtoValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Author name is required.")
           .MinimumLength(2).WithMessage("Book name must be at least 2 characters.");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Author surname is required.")
            .MinimumLength(2).WithMessage("Author surname must be at least 2 characters.");

        RuleFor(x => x.Summary)
           .NotEmpty().WithMessage("Author summary is required.")
           .MinimumLength(2).WithMessage("Author summary must be at least 100 characters.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .LessThan(DateTime.Now).WithMessage("Birth date cannot be in the future.");

        RuleFor(x => x.DeathDate)
           .GreaterThan(x => x.BirthDate)
           .When(x => x.DeathDate != null).WithMessage("Death date must be after birth date.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .Must(country => CountryList.ValidCountries.Contains(country))
            .WithMessage("Invalid country name. Please select from the valid list (e.g., 'Turkey', 'United States').");


        RuleFor(x => x.WebSiteUrl)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.WebSiteUrl)) // Sadece doluysa kontrol et
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out var outUri)
                    && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("Valid Website URL is required (must start with http:// or https://).");

    }
}