using Microsoft.EntityFrameworkCore;
using LibrarySystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence;

// 1. DÜZELTME: 'class' kelimesi eklendi.
public class LibraryDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public LibraryDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BookCopy> BookCopies { get; set; }
    public DbSet<BookSummary> BookSummaries { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    
    // 2. DÜZELTME: 'DbSet<User> Users' satırını sildim.
    // Çünkü IdentityDbContext bunu zaten içinde barındırıyor. Tekrar yazarsan hata alırsın.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Identity tabloları (User, Role, Claim vb.) için bu satır ŞART!
        base.OnModelCreating(modelBuilder);

        // --- SENİN CONFIG AYARLARIN ---

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

        // 1-e-1 İLİŞKİ
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Summary)
            .WithOne(s => s.Book)
            .HasForeignKey<BookSummary>(s => s.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // GUID INDEX AYARI
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserGuid)
            .IsUnique();
    }
}