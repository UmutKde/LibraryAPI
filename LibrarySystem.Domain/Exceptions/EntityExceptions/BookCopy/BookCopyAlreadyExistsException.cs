namespace LibrarySystem.Domain.Exceptions;

public class BookCopyAlreadyExistsException : ConflictException
{
    public BookCopyAlreadyExistsException(int BookCopyId)
        :base($"The bookcopy with id: {BookCopyId} already exists.")
    {
    }
}