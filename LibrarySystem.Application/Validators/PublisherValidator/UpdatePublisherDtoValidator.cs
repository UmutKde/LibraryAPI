using FluentValidation;
using LibrarySystem.Application.Dtos;
using LibrarySystem.Domain.Constants;

namespace LibrarySystem.Application.Validators;

public class UpdatePublisherDtoValidator : AbstractValidator<PublisherDtoForInsertion>
{
    public UpdatePublisherDtoValidator()
    {
        RuleFor(x => x.PublisherName)
         .NotEmpty().WithMessage("Author name is required.")
         .MinimumLength(2).WithMessage("Book name must be at least 2 characters.");

        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("Contact mail is required")
            .EmailAddress().WithMessage("Valid email is requied.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .Must(country => CountryList.ValidCountries.Contains(country))
            .WithMessage("Invalid country name. Please select from the valid list (e.g., 'Turkey', 'United States').");
       
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Length(10).WithMessage("Phone number must be at 10 digits.");

        RuleFor(x => x.ContactPerson)
            .NotEmpty().WithMessage("Contact person is required.")
            .MinimumLength(2).WithMessage("Contact person must be at least 2 characters.");

        RuleFor(x => x.WebsiteUrl)
         .NotEmpty().When(x => !string.IsNullOrEmpty(x.WebsiteUrl))
         .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out var outUri)
                 && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
         .WithMessage("Valid Website URL is required (must start with http:// or https://).");

    }
}