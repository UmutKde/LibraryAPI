namespace LibrarySystem.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string? CategoryName { get; set; }

    // Relation Tables
    public ICollection<Book> Books { get; set; }
}