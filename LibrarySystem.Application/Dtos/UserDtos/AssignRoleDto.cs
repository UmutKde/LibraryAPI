using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Application.Dtos;

public record AssignRoleDto
(
    [Required(ErrorMessage ="Email is required.")]
    [EmailAddress]
    string Email,

    [Required(ErrorMessage ="Role is required.")]
    List<string> Roles
);