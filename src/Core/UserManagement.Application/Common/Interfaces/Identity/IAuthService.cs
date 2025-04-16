using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Common.Interfaces.Identity;

public interface IAuthService
{
    Task<string> GenerateJwtTokenAsync(ApplicationUser user);
    bool IsPasswordExpired(ApplicationUser user, int expiryDays = 90);
}
