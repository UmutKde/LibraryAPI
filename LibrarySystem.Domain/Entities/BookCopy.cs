namespace LibrarySystem.Domain.Entities;

public class BookCopy
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public bool isAvailable { get; set; }

    // Relation Tables
    public Book Book { get; set; }
    public ICollection<Loan> Loans { get; set; }
}
