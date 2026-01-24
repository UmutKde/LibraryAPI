using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges);

    Task<Book> GetOneBookWithDetailsAsync(int id,bool trackChanges);    

} 