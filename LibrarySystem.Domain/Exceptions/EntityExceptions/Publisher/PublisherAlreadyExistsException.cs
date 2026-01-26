namespace LibrarySystem.Domain.Exceptions;

public class PublisherAlreadyExistsException : ConflictException
{
    public PublisherAlreadyExistsException(string publisherName)
        : base($"The publisher with name : {publisherName} already exists.")
    {
    }
}