namespace LibrarySystem.Application.Dtos;

public class LoanResponseDto
{
    public string UserNameSurname { get; set; }
    public string Barcode { get; set; }
    public DateTime DueDate { get; init; }
    public string Message { get; init; }
}