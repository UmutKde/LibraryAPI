using LibrarySystem.Application.Dtos;
using Microsoft.AspNetCore.Identity;
namespace LibrarySystem.Application.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserRegistrationDto userRegistrationDto);

    Task<TokenDto> Login(UserLoginDto userLoginDto);
    Task<bool> AssignRoleToUser(AssignRoleDto assingRoleDto);
    Task<bool> AssignPermissionToRole(AssignPermissionDto assignPermissionDto);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
    Task<bool> RefreshPassword(string email,string newPassword);
}