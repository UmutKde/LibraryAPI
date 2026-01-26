namespace LibrarySystem.Application.Dtos;

public class BookCopyDto
{
    public int Id { get; set; }
    public string Barcode { get; set; }
    public string Condition { get; set; }
    public bool IsAvailable { get; set; }
    public string BookTitle { get; set; }
}