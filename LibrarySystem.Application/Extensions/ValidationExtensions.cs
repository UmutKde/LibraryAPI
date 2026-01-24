using FluentValidation;
using FluentValidation.AspNetCore;
using LibrarySystem.Application.Validators;
using LibrarySystem.Domain.ErrorModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Application.Extensions;

public static class ValidationExtensions
{
    public static void AddValidationExtensions(this IServiceCollection services)
    {
        // 1. FluentValidation'ın otomatik doğrulama özelliğini açıyoruz.
        // Bu sayede Controller'a girmeden kuralları kontrol eder.
        services.AddFluentValidationAutoValidation();
        
        // 2. Client-side adapter (Opsiyonel, genelde MVC için kullanılır ama zararı yok)
        services.AddFluentValidationClientsideAdapters();

        // 3. KRİTİK NOKTA: Validatorları Kaydetme 
        // "CreateBookDtoValidator" sınıfının bulunduğu Assembly'deki (yani bu katmandaki)
        // tüm AbstractValidator sınıflarını bul ve sisteme tanıt.
        services.AddValidatorsFromAssemblyContaining<CreateBookDtoValidator>();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();
                    
                var response = new ValidationErrorDetails()
                {
                    StatusCode = 422,
                    Message = "One or more validation errors occurred.",
                    Errors = errors
                };
                return new UnprocessableEntityObjectResult(response);
            };
        });
    }
}