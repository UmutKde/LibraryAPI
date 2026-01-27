namespace LibrarySystem.Domain.Entities;

public class Loan
{
    public int Id { get; set; }
    public DateTime LoanDate { get; set; } // kiralanma tarihi
    public DateTime DueDate { get; set; } // geri dönmesi gereken süre 
    public DateTime? ReturnDate { get; set; }  // döndü mü - dönmedi ise null döndü ise 

    // Relation Tables
    public int UserId { get; set; }
    public int BookCopyId { get; set; }
    public User User { get; set; }
    public BookCopy BookCopy { get; set; }
}
