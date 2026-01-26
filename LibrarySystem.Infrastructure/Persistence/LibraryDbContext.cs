using Microsoft.EntityFrameworkCore;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Infrastructure.Persistence;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BookCopy> BookCopies { get; set; }
    public DbSet<BookSummary> BookSummaries { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();


        modelBuilder.Entity<Category>()
            .HasIndex(c => c.CategoryName)
            .IsUnique();


        modelBuilder.Entity<Publisher>()
            .HasIndex(p => p.PublisherName)
            .IsUnique();

        modelBuilder.Entity<Author>()
            .HasIndex(a => new { a.Name, a.Surname })
            .IsUnique();

        // --- 1-e-1 İLİŞKİ KURULUMU ---
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Summary)
            .WithOne(s => s.Book)
            .HasForeignKey<BookSummary>(s => s.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}