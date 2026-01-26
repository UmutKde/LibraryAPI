using LibrarySystem.Application.Dtos;

namespace LibrarySystem.Application.Interfaces;

public interface IBookCopyService
{
    Task<IEnumerable<string>> CreateCopiesAsync(BookCopyDtoForInsertion bookCopyDtoForInsertion);
    Task UpdateCopyAsync(BookCopyDtoForUpdate bookCopyDtoForUpdate);
    Task DeleteCopyAsync(int id);
    Task<IEnumerable<BookCopyDto>> GetAllCopiesByBookIdAsync(int bookId);
}