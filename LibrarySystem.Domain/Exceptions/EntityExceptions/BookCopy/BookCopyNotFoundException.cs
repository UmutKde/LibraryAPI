namespace LibrarySystem.Domain.Exceptions;

public class BookCopyNotFoundException : NotFoundException
{
    public BookCopyNotFoundException(string barcode)
        :base($"The bookcopy with barcode: {barcode} could not be found.")
    {
    }
    public BookCopyNotFoundException(int id)
        : base($"The book copy with id: {id} could not be found.")
    {
    }
}