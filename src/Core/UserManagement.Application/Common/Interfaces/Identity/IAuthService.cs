using UserManagement.Application.Features.Auth.Dtos;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Common.Interfaces.Identity;

public interface IAuthService
{
    Task<TokenResponse> GenerateJwtTokenAsync(ApplicationUser user);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    bool IsPasswordExpired(ApplicationUser user, int expiryDays = 90);
}
