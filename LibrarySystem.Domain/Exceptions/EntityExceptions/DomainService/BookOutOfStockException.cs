namespace LibrarySystem.Domain.Exceptions;

public class BookOutOfStockException : ConflictException
{
    public BookOutOfStockException(int bookId)
        : base($"No available copies found for book with id: {bookId}.")
    {
    }
}