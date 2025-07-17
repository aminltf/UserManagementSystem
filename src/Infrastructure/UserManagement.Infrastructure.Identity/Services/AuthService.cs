#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Application.Common.Interfaces.Identity;
using UserManagement.Application.Features.Auth.Dtos;
using UserManagement.Domain.Entities.Identity;
using UserManagement.Infrastructure.Identity.Contexts;
using UserManagement.Shared.Kernel.Settings;

namespace UserManagement.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IdentityContext _identityContext;

    public AuthService(IOptions<JwtSettings> options, UserManager<ApplicationUser> userManager, IdentityContext identityContext)
    {
        _jwtSettings = options.Value;
        _userManager = userManager;
        _identityContext = identityContext;
    }

    public async Task<TokenResponse> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? ""),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);
        user.IsRefreshTokenRevoked = false;

        await _userManager.UpdateAsync(user);

        return new TokenResponse(accessToken, refreshToken);
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var user = await _identityContext.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow || user.IsRefreshTokenRevoked)
        {
            throw new SecurityTokenException("Invalid refresh token.");
        }

        user.IsRefreshTokenRevoked = true;
        await _userManager.UpdateAsync(user);

        return await GenerateJwtTokenAsync(user);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public bool IsPasswordExpired(ApplicationUser user, int expiryDays = 90)
    {
        return !user.PasswordChangedAt.HasValue ||
               (DateTime.UtcNow - user.PasswordChangedAt.Value).TotalDays > expiryDays;
    }
}
