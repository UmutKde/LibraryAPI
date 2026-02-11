using LibrarySystem.Application.Dtos;
using Microsoft.AspNetCore.Identity;
namespace LibrarySystem.Application.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserRegistrationDto userRegistrationDto);

    Task<string> Login(UserLoginDto userLoginDto);
}