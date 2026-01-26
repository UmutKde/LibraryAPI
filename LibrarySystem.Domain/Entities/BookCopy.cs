namespace LibrarySystem.Domain.Entities;

public class BookCopy
{
    public int Id { get; set; }
    public string Barcode { get; set; } = Guid.NewGuid().ToString().Substring(0,8).ToUpper();
    public bool IsAvailable { get; set; } = true;
    public string Condition { get; set; } = "New";
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public decimal? ReplacementCost { get; set; }
    
    // Relation Tables
    public int BookId { get; set; }
    public Book Book { get; set; }
    public ICollection<Loan> Loans { get; set; }
}
