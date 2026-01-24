using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence;

public static class LibraryDbContextSeed
{
    public static async Task SeedAsync(LibraryDbContext context)
    {
        // 1. Veritabanı yoksa oluştur veya migrationları uygula
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Migration hatası (Zaten güncel olabilir): " + ex.Message);
        }

        // 2. Eğer içeride veri varsa tekrar ekleme yapma (Çıkış)
        if (context.Authors.Any() || context.Books.Any())
        {
            return;
        }

        Console.WriteLine("--> Seed Data yükleniyor...");

        // --- ADIM 1: REFERANS VERİLER (Yazar, Kategori, Yayınevi) ---

        // A) Yazarlar (Yeni Entity yapısına uygun)
        var authorDostoyevski = new Author
        {
            Name = "Fyodor",
            Surname = "Dostoyevski",
            BirthDate = new DateTime(1821, 11, 11, 0, 0, 0, DateTimeKind.Utc),
            DeathDate = new DateTime(1881, 2, 9, 0, 0, 0, DateTimeKind.Utc),
            Country = "Russia", // Validasyon listemize uygun
            Summary = "Rus edebiyatının en büyük temsilcilerinden.",
            ImageUrl = null
        };

        var authorTolkien = new Author
        {
            Name = "J.R.R.",
            Surname = "Tolkien",
            BirthDate = new DateTime(1892, 1, 3, 0, 0, 0, DateTimeKind.Utc),
            DeathDate = new DateTime(1973, 9, 2, 0, 0, 0, DateTimeKind.Utc),
            Country = "United Kingdom", // Validasyon listemize uygun
            Summary = "Fantastik edebiyatın babası.",
            ImageUrl = null
        };

        // B) Kategoriler
        var categoryRoman = new Category { CategoryName = "Roman" };
        var categoryFantastik = new Category { CategoryName = "Fantastik" };
        var categoryTarih = new Category { CategoryName = "Tarih" };

        // C) Yayınevleri
        var publisherYKY = new Publisher { PublisherName = "Yapı Kredi Yayınları" };
        var publisherMetis = new Publisher { PublisherName = "Metis Yayınları" };

        // --- ADIM 2: İLİŞKİLERİ KUR VE KİTAPLARI OLUŞTUR ---
        // Not: ID vermiyoruz, EF Core nesneleri birbirine bağladığımızı anlayıp ID'leri kendi atayacak.

        var books = new List<Book>
        {
            new Book
            {
                BookName = "Suç ve Ceza",
                ISBN = "9789753638059",
                PageCount = 687,
                ImageUrl = null,
                Publisher = publisherYKY, // Nesne olarak atadık
                Authors = new List<Author> { authorDostoyevski }, // Listeye ekledik
                Categories = new List<Category> { categoryRoman }
            },
            new Book
            {
                BookName = "Yüzüklerin Efendisi - Yüzük Kardeşliği",
                ISBN = "9789753420845",
                PageCount = 496,
                ImageUrl = null,
                Publisher = publisherMetis,
                Authors = new List<Author> { authorTolkien },
                Categories = new List<Category> { categoryRoman, categoryFantastik } // Birden fazla kategori örneği
            }
        };

        // --- ADIM 3: KAYDETME ---
        // Sadece Kitapları eklemek yeterli, EF Core bağlı olduğu Yazar ve Kategorileri de otomatik ekler.
        // Ama garanti olsun diye hepsini context'e tanıtabiliriz.

        await context.Authors.AddRangeAsync(authorDostoyevski, authorTolkien);
        await context.Categories.AddRangeAsync(categoryRoman, categoryFantastik, categoryTarih);
        await context.Publishers.AddRangeAsync(publisherYKY, publisherMetis);
        await context.Books.AddRangeAsync(books);

        await context.SaveChangesAsync();

        Console.WriteLine("--> Seed Data başarıyla eklendi. 🚀");
    }
}