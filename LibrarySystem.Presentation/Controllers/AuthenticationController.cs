using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
private readonly IAuthenticationService _service;

    public AuthenticationController(IAuthenticationService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegistrationDto)
    {
        var result = await _service.RegisterUser(userRegistrationDto);
        if(result.Succeeded)
        {
            foreach(var error in result.Errors)
            {
                
            }
            return StatusCode(201);
        }
        foreach(var error in result.Errors)
        {
            ModelState.TryAddModelError(error.Code, error.Description);
        }
        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        try
        {
            var token = await _service.Login(userLoginDto);
            return Ok(new {Token = token});
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }

    [Authorize]
    [HttpGet("test-token")]
    public IActionResult TestToken()
    {
        // Eğer buraya kadar gelebildiyse, Token geçerli demektir.
        return Ok("Tebrikler! 🔓 Kapı açıldı, Token geçerli. İçeridesin!");
    }
}