namespace LibrarySystem.Application.Dtos;

public class LoanDto
{
    public int Id { get; set; }
    
    // İlişkisel Veriler (İnsanlar için)
    public string UserName { get; set; }      // "Ahmet Yılmaz"
    public string BookName { get; set; }     // "Dune"
    public string Barcode { get; set; }       // "8059-XC"
    
    // Tarihler
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; } // İade edilmediyse null
}