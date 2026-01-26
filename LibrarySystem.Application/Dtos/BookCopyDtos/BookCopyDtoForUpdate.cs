namespace LibrarySystem.Application.Dtos;

public class BookCopyDtoForUpdate
{
    public int Id { get; set; }
    public string Condition { get; set; }
    public bool IsAvailable { get; set; }
    public decimal? ReplacementCost { get; set; }
}