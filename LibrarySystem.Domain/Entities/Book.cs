namespace LibrarySystem.Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string? BookName { get; set; }
    public string? ISBN { get; set; }
    public string? ImageUrl { get; set; }
    public int PageCount { get; set; }
    public int PublisherId { get; set; }


    // Relation Tables
    public ICollection<Category> Categories { get; set; }
    public ICollection<Author> Authors { get; set; }
    public ICollection<BookCopy> BookCopies { get; set; }
    public Publisher Publisher { get; set; }
    public BookSummary Summary { get; set; }

    public Book()
    {
        Authors = new List<Author>();
        Categories = new List<Category>();
        BookCopies = new List<BookCopy>();
    }

}