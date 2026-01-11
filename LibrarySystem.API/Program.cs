using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Extensions DI
builder.Services.AddInfrastructureService(builder.Configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Infrastructure katmanındaki context'i çağırıyoruz
        var context = services.GetRequiredService<LibraryDbContext>();
        
        // Yazdığımız tohumlama metodunu çalıştırıyoruz
        await LibraryDbContextSeed.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı seed edilirken bir hata oluştu.");
    }
}

app.Run();
