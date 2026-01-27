namespace LibrarySystem.Application.Dtos;

public class LoanResponseDto
{
    public int LoanId { get; init; }
    public string UserNameSurname { get; set; }
    public string Barcode { get; set; }
    public DateTime DueDate { get; init; }
    public string Message { get; init; }
}