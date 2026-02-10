using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Application.Dtos; // Namespace'i Dtos klasörüne taşıdım

public record UserRegistrationDto
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; init; }

    [Required(ErrorMessage = "Surname is required.")]
    public string Surname { get; init; }

    [Required(ErrorMessage = "Username is required.")]
    public string UserName { get; init; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; init; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; init; }

    [Required(ErrorMessage = "Phone number is required.")]
    public string PhoneNumber { get; init; }
    
    [Required] 
    public DateOnly BirthDate { get; init; }

    public bool Gender { get; init; }

    public ICollection<string>? Roles { get; init; }
}