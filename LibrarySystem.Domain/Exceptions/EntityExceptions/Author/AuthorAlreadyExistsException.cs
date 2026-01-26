namespace LibrarySystem.Domain.Exceptions;

public class AuthorAlreadyExistsException : ConflictException
{
    public AuthorAlreadyExistsException(string nameSurname)
        : base($"The author with name : {nameSurname} already exists.")
    {
    }
}