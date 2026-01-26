namespace LibrarySystem.Domain.Entities;

public class Loan
{
    public int Id { get; set; }
    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    // Relation Tables
    public int UserId { get; set; }
    public int BookCopyId { get; set; }
    public User User { get; set; }
    public BookCopy BookCopy { get; set; }
}
