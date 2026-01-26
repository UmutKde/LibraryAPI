namespace LibrarySystem.Domain.Exceptions;

public class BookCopyNotAvailableException : ConflictException
{
    public BookCopyNotAvailableException(int bookCopyId)
        :base($"Book copy with id: {bookCopyId} is currnetly on loan and cannot be deleted.")
    { 
    }
}