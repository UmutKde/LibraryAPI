namespace LibrarySystem.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; } 
    public string? Email { get; set; }
    public DateTime BirthDate { get; set; }
    public bool Gender { get; set; }

    // Relation Tables
    public ICollection<Loan> Loans { get; set; }
}