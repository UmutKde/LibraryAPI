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
        if (result.Succeeded)
        {
            foreach (var error in result.Errors)
            {

            }
            return StatusCode(201);
        }
        foreach (var error in result.Errors)
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
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(new 
            { 
                Mesaj = "Sistem bir yerde patladı!", 
                HataNedeni = ex.Message, 
                İçDetay = ex.InnerException?.Message 
            });
        }
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        try
        {
            var newTokenDto = await _service.RefreshToken(tokenDto);
            return Ok(newTokenDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new {Message = ex.Message});
        }
    }

    [HttpPost]
    [Route("assign-permission")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionDto assignPermissionDto)
    {
        var result = await _service.AssignPermissionToRole(assignPermissionDto);
        if(result)
            return Ok(new {Message = $"The {assignPermissionDto.Permission} permission has been successfully added to the {assignPermissionDto.RoleName} role!"});
        
        return BadRequest("Operation failed! Role not found or permission could not be added.");

    }

    [Authorize]
    [HttpGet("test-token")]
    public IActionResult TestToken()
    {
        // Eğer buraya kadar gelebildiyse, Token geçerli demektir.
        return Ok("Tebrikler! 🔓 Kapı açıldı, Token geçerli. İçeridesin!");
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto assignRoleDto)
    {
        var result = await _service.AssignRoleToUser(assignRoleDto);
        if (result)
        {
            return Ok(new { Message = "Roller başarıyla atandı!" });
        }

        return BadRequest("İşlem başarısız! Kullanıcı bulunamadı veya gönderdiğin rollerden biri veritabanında (AspNetRoles) yok.");
    }

    [HttpPost("force-reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForceResetPassword([FromQuery] string email, [FromQuery] string newPassword)
    {
        var isSuccess = await _service.RefreshPassword(email,newPassword);

        if(!isSuccess)
            return BadRequest("Kullanıcı bulunamadı.");
        
        return Ok("Şifre başarıyla sıfırlandı.");
    }
}