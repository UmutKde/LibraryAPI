using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
    {
        var query = _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Categories);
        return trackChanges
        ? await query.ToListAsync()
        : await query.AsNoTracking().ToListAsync();
    }

    public async Task<Book> GetOneBookWithDetailsAsync(int id,bool trackChanges)
    {
        var query = _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Categories)
            .Include(b => b.Summary)
            .Where(b => b.Id == id);
        return trackChanges
        ? await query.SingleOrDefaultAsync()
        : await query.AsNoTracking().SingleOrDefaultAsync();
    }
}