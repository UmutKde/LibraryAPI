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

    public AuthenticationService(UserManager<User> userManager, IMapper mapper, ITokenService tokenService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<string> Login(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
        if(user == null || !await _userManager.CheckPasswordAsync(user,userLoginDto.Password)) 
            throw new Exception("Invalid Email or Password.");
        
        return await _tokenService.CreateTokenAsync(user);
    }

    public async Task<IdentityResult> RegisterUser(UserRegistrationDto userRegistrationDto)
    {
        var user = _mapper.Map<User>(userRegistrationDto);
        var result = await _userManager.CreateAsync(user,userRegistrationDto.Password);

        if(result.Succeeded && userRegistrationDto.Roles != null && userRegistrationDto.Roles.Count > 0)
        {
            await _userManager.AddToRolesAsync(user,userRegistrationDto.Roles);
        }
        return result;
    }
}