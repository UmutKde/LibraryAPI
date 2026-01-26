using LibrarySystem.Domain.Exceptions;

namespace LibrarySystem.Domain.Entities;

public sealed class PublisherNotFoundException : NotFoundException
{
    public PublisherNotFoundException(int id)
        : base($"The publisher with id: {id} could not found.")
    {
    }    
}