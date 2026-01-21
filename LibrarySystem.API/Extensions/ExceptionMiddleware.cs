using System.Net;
using LibrarySystem.API.Models;

namespace LibrarySystem.API.Extensions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;


    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Bir şeyler ters gitti : {ex}");
            await HandleExceptionAsync(httpContext,ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context,Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        };
        await context.Response.WriteAsync(errorResponse.ToString());
    }
}