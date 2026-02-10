using LibrarySystem.API.Extensions;
using LibrarySystem.Application.Extensions;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Persistence;
using LibrarySystem.Presentation;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

// Extensions DI
builder.Services.AddInfrastructureService(builder.Configuration);
// App Extension DI
builder.Services.AddApplicationService();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Controller Kullanmak için
builder.Services.AddControllers()
    .AddApplicationPart(typeof(AssemblyReference).Assembly);

// FluentValidation
builder.Services.AddValidationExtensions();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers(); 

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
