namespace LibrarySystem.Domain.Entities;

public class Author
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string?  ImageUrl { get; set; }
    public string? Summary { get; set; }

    // Relation Tables
    public ICollection<Book> Books { get; set; }
}