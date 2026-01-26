namespace LibrarySystem.Domain.Exceptions;

public class BookCopyNotFoundException : NotFoundException
{
    public BookCopyNotFoundException(int bookCopyId)
        :base($"The bookcopy with id: {bookCopyId} could not found.")
    {
    }
}