using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    // 1. 👇 RoleManager'ı tanımlıyoruz (Eğer Rol sınıfın farklıysa <IdentityRole> olarak değiştirebilirsin)
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    // 2. 👇 Constructor'a RoleManager'ı ekliyoruz
    public TokenService(IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public string CreateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<string> CreateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));

            var role = await _roleManager.FindByNameAsync(userRole);
            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in roleClaims)
                {
                    claims.Add(claim);
                }
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:secretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:validIssuer"],
            audience: _configuration["JwtSettings:validAudience"],
            claims: claims,
            // 4. 👇 DİKKAT: .Now yerine .UtcNow yaptık! (Sunucu saati krizlerini önlemek için)
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:expires"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = _configuration["JwtSettings:validAudience"],
            ValidIssuer = _configuration["JwtSettings:validIssuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:secretKey"]!)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token,tokenValidationParameters, out SecurityToken securityToken);
        var JwtSecurityToken = securityToken as JwtSecurityToken;
        
        if(JwtSecurityToken == null || !JwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Geçersiz Token");

        return principal;
    }
}