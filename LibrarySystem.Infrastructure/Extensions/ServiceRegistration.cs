using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Infrastructure.Persistence;
using LibrarySystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

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

        // Unit Of Work Kaydı
        services.AddScoped<IUnitOfWork,UnitOfWork>();

        services.AddIdentity<User, IdentityRole<int>>(opt => 
        {
            opt.Password.RequireDigit = true;
            opt.Password.RequiredLength = 8;
            opt.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<LibraryDbContext>()
        .AddDefaultTokenProviders();
    }
}