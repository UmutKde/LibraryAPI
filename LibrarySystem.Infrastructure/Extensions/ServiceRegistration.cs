using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Infrastructure.Persistence;
using LibrarySystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureService(this IServiceCollection services,IConfiguration configuration)
    {
        // Veri tabanı bağlantısı
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));   

        // Generic Repository Kaydı
        services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
    }
}