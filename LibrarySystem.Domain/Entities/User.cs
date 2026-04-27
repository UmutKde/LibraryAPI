using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.Domain.Entities;

public class User : IdentityUser<int>
{
    public Guid UserGuid { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool Gender { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Relation Tables
    public ICollection<Loan> Loans { get; set; }
}