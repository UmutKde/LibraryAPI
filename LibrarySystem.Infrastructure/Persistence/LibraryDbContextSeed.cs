using LibrarySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence;

public static class LibraryDbContextSeed
{
    public static async Task SeedAsync(LibraryDbContext context)
    {
        try 
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Migration hatası: " + ex.Message);
        }

        // Eğer içeride kitap varsa çık (Zaten veri var demektir)
        if (context.Books.Any())
        {
            return; 
        }

        // --- ADIM 1: ÖNCE BAĞIMLILIKLARI OLUŞTUR (Yazar, Yayınevi, Kategori) ---
        // Kitabı oluşturabilmek için önce bunları veritabanına kaydetmemiz lazım.

        // 1. Kategoriler
        var romanCategory = new Category { CategoryName = "Roman" }; // Property adı 'CategoryName' veya 'Name' olabilir, kendi entity'ne göre düzelt
        var tarihCategory = new Category { CategoryName = "Tarih" };
        
        // 2. Yazarlar
        var authorDostoyevski = new Author { Name = "Fyodor Dostoyevski" }; // Property adı 'AuthorName' olabilir
        var authorTolkien = new Author { Name = "J.R.R. Tolkien" };

        // 3. Yayınevleri
        var publisherYKY = new Publisher { PublisherName = "Yapı Kredi Yayınları" }; // Property adı 'PublisherName' olabilir
        var publisherMetis = new Publisher { PublisherName = "Metis Yayınları" };

        // Bunları önce context'e ekleyelim ki ID'leri oluşsun (Henüz SaveChanges yapmıyoruz, EF Core hafızada takip edecek)
        // Not: Eğer DbContext'inde bu DbSet'ler tanımlı değilse hata alırsın. Tanımlı olduklarını varsayıyorum.
        await context.Categories.AddRangeAsync(romanCategory, tarihCategory);
        await context.Authors.AddRangeAsync(authorDostoyevski, authorTolkien);
        await context.Publishers.AddRangeAsync(publisherYKY, publisherMetis);
        
        // --- ADIM 2: KİTAPLARI OLUŞTUR VE BAĞLA ---
        
        var books = new List<Book>
        {
            new Book 
            { 
                BookName = "Suç ve Ceza", 
                ISBN = "1234567890", 
                PageCount = 687,
                ImageUrl = null,
                
                // İlişkileri Nesne Olarak Veriyoruz (ID vermeye gerek yok, EF anlar)
                Publisher = publisherYKY, 
                Authors = new List<Author> { authorDostoyevski },
                Categories = new List<Category> { romanCategory },
                
                // BookCopies ve Summary şimdilik boş kalabilir veya eklenebilir
                BookCopies = new List<BookCopy>(), 
            },
            new Book 
            { 
                BookName = "Yüzüklerin Efendisi", 
                ISBN = "0987654321", 
                PageCount = 1024,
                ImageUrl = null,
                
                Publisher = publisherMetis, 
                Authors = new List<Author> { authorTolkien },
                Categories = new List<Category> { romanCategory },
                
                BookCopies = new List<BookCopy>()
            }
        };

        await context.Books.AddRangeAsync(books);
        
        // --- ADIM 3: HEPSİNİ TEK SEFERDE KAYDET ---
        await context.SaveChangesAsync();
    }
}