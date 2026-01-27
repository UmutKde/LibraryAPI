using LibrarySystem.Domain.Constants;

namespace LibrarySystem.Application.Dtos;

public class LoanDtoForUpdate
{
    public int Id { get; set; }
    public string Barcode { get; set; }
    public int UserId { get; set; }
    public BookCondition Condition { get; set; }

}