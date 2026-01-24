using LibrarySystem.Application.Dtos;

namespace LibrarySystem.Application.Interfaces;

public interface IPublisherService
{
    Task<IEnumerable<PublisherDto>> GetAllPublisherAsync();
    Task<PublisherDto> GetPublisherByIdAsync(int id);
    Task<PublisherDto> CreatePublisherAsync(PublisherDtoForInsertion publisherDtoForInsertion);
    Task UpdatePublisherAsync(PublisherDtoForUpdate publisherDtoForUpdate);
    Task DeletePublisherAsync(int id);
}