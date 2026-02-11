using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Application.Dtos;

public record UserLoginDto(
    
    [Required(ErrorMessage ="Email is required.")]
    [EmailAddress]
    string Email,

    [Required(ErrorMessage ="Password is required.")]
    string Password
);