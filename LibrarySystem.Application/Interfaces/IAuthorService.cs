using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Dtos.BookDtos;

namespace LibrarySystem.Application.Interfaces;

public interface IAuthorService
{
    Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
    Task<AuthorDto> GetAuthorByIdAsync(int id);
    Task<AuthorDto> CreateAuthorAsync(AuthorDtoForInsertion authorDtoForInsertion);
    Task UpdateAuthorAsync(AuthorDtoForUpdate authorDtoForUpdate);
    Task DeleteAuthorAsync(int id);
}