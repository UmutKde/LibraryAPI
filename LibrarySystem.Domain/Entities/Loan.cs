namespace LibrarySystem.Domain.Entities;

public class Loan
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookCopyId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    // Relation Tables
    public User User { get; set; }
    public int BookId { get; set; }
    public BookCopy BookCopy { get; set; }
}
