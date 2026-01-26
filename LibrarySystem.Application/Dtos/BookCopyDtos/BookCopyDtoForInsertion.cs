namespace LibrarySystem.Application.Dtos;

public class BookCopyDtoForInsertion
{
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public decimal? ReplacementCost { get; set; }
    public string? Condition { get; set; }
}