using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync();

    Task<Book> GetOneBookWithDetailsAsync(int id);    

} 