using System.Security.Claims;
using AutoMapper;
using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public AuthenticationService(UserManager<User> userManager, IMapper mapper, ITokenService tokenService, RoleManager<IdentityRole<int>> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }

    public async Task<TokenDto> Login(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            throw new Exception("Invalid Email or Password.");

        var accessToken = await _tokenService.CreateTokenAsync(user);
        var refrestToken = _tokenService.CreateRefreshToken();

        user.RefreshToken = refrestToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);
        return new TokenDto(accessToken, refrestToken);

    }

    public async Task<IdentityResult> RegisterUser(UserRegistrationDto userRegistrationDto)
    {
        var user = _mapper.Map<User>(userRegistrationDto);
        var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }
        return result;
    }

    public async Task<bool> AssignRoleToUser(AssignRoleDto assingRoleDto)
    {
        var user = await _userManager.FindByEmailAsync(assingRoleDto.Email);

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
            return false;

        var result = await _userManager.AddToRolesAsync(user, assingRoleDto.Roles);
        return result.Succeeded;
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var userName = principal.Identity?.Name;

        var user = await _userManager.FindByNameAsync(userName!);
        if (user == null)
            throw new Exception("User not found.");

        if (user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new Exception("Your refresh token is invalid or has expired. Please log in again.");

        var newAccessToken = await _tokenService.CreateTokenAsync(user);
        var newRefreshToken = _tokenService.CreateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new TokenDto(newAccessToken, newRefreshToken);

    }

    public async Task<bool> AssignPermissionToRole(AssignPermissionDto assignPermissionDto)
    {
        var role = await _roleManager.FindByNameAsync(assignPermissionDto.RoleName);
        
        if(role == null)
            return false;
        var existingClaims = await _roleManager.GetClaimsAsync(role);
        if(existingClaims.Any(c => c.Type == "Permission" && c.Value == assignPermissionDto.Permission))
            return true;
        var result = await _roleManager.AddClaimAsync(role,new Claim("Permission", assignPermissionDto.Permission));
        return result.Succeeded;
    }

    public async Task<bool> RefreshPassword(string email, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if(user == null)
            return false;

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user,resetToken,newPassword);

        return result.Succeeded;

    }
}