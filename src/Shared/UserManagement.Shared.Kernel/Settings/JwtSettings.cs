#nullable disable

namespace UserManagement.Shared.Kernel.Settings;

public class JwtSettings
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public double ExpiryMinutes { get; set; }
    public int RefreshTokenExpiryDays { get; set; }
}
