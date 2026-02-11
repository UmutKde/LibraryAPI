using System.Reflection;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Application.Extensions;

public static class ServiceExtensions
{
    public static void AddApplicationService(this IServiceCollection services)
    {
        // AutoMapper Kaydı
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Service Kaydı
        services.AddScoped<IBookService,BookService>();
        services.AddScoped<ICategoryService,CategoryService>();
        services.AddScoped<IAuthorService,AuthorService>();
        services.AddScoped<IPublisherService,PublisherService>();
        services.AddScoped<IBookCopyService,BookCopyService>();
        services.AddScoped<ILoanService,LoanService>();
        services.AddScoped<IAuthenticationService,AuthenticationService>();
        services.AddScoped<ITokenService,TokenService>();
    }
}