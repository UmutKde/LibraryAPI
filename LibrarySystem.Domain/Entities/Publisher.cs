namespace LibrarySystem.Domain.Entities;

public class Publisher
{
    public int Id { get; set; }
    public string? PublisherName { get; set; }
    public string? ImageUrl { get; set; }

    // Relation Table
    public ICollection<Book> Books { get; set; }
}