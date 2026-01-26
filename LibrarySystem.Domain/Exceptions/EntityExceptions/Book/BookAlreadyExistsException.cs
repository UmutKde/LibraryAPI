namespace LibrarySystem.Domain.Exceptions;

public class BookAlreadyExistsException : ConflictException
{
    public BookAlreadyExistsException(string ISBN)
        : base($"The book with ISBN: {ISBN} already exists.")
    {
    }
}