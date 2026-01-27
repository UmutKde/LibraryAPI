using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBookRepository Books {get;}

    IGenericRepository<Author> Authors {get;}
    IGenericRepository<Publisher> Publishers {get;}
    IGenericRepository<Category> Categories {get;}
    IGenericRepository<BookCopy> BookCopies {get;}
    IGenericRepository<Loan> Loans {get;}
    IGenericRepository<User> Users {get;}

    Task<int> SaveChangesAsync();
}