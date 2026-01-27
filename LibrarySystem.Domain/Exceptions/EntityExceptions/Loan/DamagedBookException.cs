namespace LibrarySystem.Domain.Exceptions;

public class DamagedBookException : ConflictException
{
    public DamagedBookException()
        : base($"Book copy is damaged. Please initiate legal proceedings.")
    {
    }
}