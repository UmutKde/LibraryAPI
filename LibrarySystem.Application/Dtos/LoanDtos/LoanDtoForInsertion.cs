namespace LibrarySystem.Application.Dtos;

public class LoanDtoForInsertion
{
    public int Id { get; set; }
    public string Barcode { get; set; }
    public int UserId { get; set; }
}