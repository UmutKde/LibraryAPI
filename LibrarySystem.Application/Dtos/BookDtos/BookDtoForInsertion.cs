namespace LibrarySystem.Application.Dtos.BookDtos;

public class BookDtoForInsertion
{
    public string BookName { get; set; }
    public string ISBN { get; set; }
    public string? ImageUrl { get; set; }
    public int PageCount { get; set; }
    public int? PublisherId { get; set; }
    public List<int> AuthorIds { get; set; }
    public List<int> CategoryIds { get; set; }
    public string Summary { get; set; }
}