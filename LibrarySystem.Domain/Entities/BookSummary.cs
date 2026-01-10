namespace LibrarySystem.Domain.Entities;

public class BookSummary
{
    public int Id { get; set; }
    public string? Summary { get; set; }
    
    // Relation Tables
    public int BookId { get; set; }
    public Book Book { get; set; }
}