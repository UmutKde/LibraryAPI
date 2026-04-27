using System.Security.Claims;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces;

public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
    string CreateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}