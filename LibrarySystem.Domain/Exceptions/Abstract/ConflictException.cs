namespace LibrarySystem.Domain.Exceptions;

public class ConflictException : Exception
{
    protected ConflictException(string message) : base(message)
    {
    }
}