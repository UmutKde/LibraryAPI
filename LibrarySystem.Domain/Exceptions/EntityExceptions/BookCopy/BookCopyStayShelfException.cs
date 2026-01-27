namespace LibrarySystem.Domain.Exceptions;

public class BookCopyStayShelfException : ConflictException
{
    public BookCopyStayShelfException(string barcode)
        : base($"Book copy with barcode: {barcode} is currnetly on shelf and cannot be loaned.")
    {
    }
}