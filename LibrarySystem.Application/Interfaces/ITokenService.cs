using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces;

public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
}