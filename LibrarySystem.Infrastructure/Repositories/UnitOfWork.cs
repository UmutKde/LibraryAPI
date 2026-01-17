using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Infrastructure.Persistence;

namespace LibrarySystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext _context;
    public IBookRepository Books {get; private set;}

    public IGenericRepository<Author> Authors {get; private set;}

    public IGenericRepository<Publisher> Publishers {get; private set;}

    public IGenericRepository<Category> Categories {get; private set;}

    public UnitOfWork(LibraryDbContext context)
    {
        _context = context;
        Books = new BookRepository(_context);
        Authors = new GenericRepository<Author>(_context);
        Publishers = new GenericRepository<Publisher>(_context);
        Categories = new GenericRepository<Category>(_context);
    }


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}