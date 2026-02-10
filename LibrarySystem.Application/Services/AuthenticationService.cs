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

    public AuthenticationService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
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