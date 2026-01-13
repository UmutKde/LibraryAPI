namespace LibrarySystem.Application.Dtos.BookDto;

public class BookDto
{
    public int Id { get; set; }
    public string BookName {get;set;}
    public string ISBN { get; set; }
    public string? ImageUrl { get; set; }
    public int PageCount { get; set; }

    public string? PublisherName { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Categories { get; set; }
}