namespace LibrarySystem.Domain.Exceptions;

public sealed class AuthorNotFoundException : NotFoundException
{
    public AuthorNotFoundException(int id)
        : base($"The author with id: {id} could not found.")
    {
    }
}