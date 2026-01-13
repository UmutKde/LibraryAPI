using LibrarySystem.Application.Dtos.BookDto;

namespace LibrarySystem.Application.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllBooksAsync();
    Task<BookDto> GetBookByIdAsync(int id);
    Task<BookDto> CreateBookAsync(BookDtoForInsertion bookDtoForInsertion);
    Task UpdateBookAsync(BookDtoForUpdate bookDtoForUpdate);
    Task DeleteBookAsync(int id);
}