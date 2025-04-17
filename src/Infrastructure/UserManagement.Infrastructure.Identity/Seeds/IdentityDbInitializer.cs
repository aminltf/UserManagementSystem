using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserManagement.Domain.Entities.Identity;
using UserManagement.Shared.Kernel.Enums;

namespace UserManagement.Infrastructure.Identity.Seeds;

public static class IdentityDbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");

        string[] roles = ["Admin", "Manager"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                logger.LogInformation("Role '{Role}' created", role);
            }
        }

        var users = new[]
        {
                new { Username = "admin", Email = "admin@system.com", Password = "Admin123!", Role = "Admin" },
                new { Username = "manager", Email = "manager@system.com", Password = "Manager123!", Role = "Manager" }
            };

        foreach (var entry in users)
        {
            if (await userManager.FindByNameAsync(entry.Username) is null)
            {
                var user = new ApplicationUser
                {
                    UserName = entry.Username,
                    Email = entry.Email,
                    Role = Enum.Parse<UserRole>(entry.Role),
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedBy = "seeder",
                    PasswordChangedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, entry.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, entry.Role);
                    logger.LogInformation("✅ User '{User}' with role '{Role}' created", entry.Username, entry.Role);
                    Console.WriteLine($"✅ Created: {entry.Username}");
                }
                else
                {
                    logger.LogError("❌ Failed to create user '{User}': {Errors}", entry.Username,
                        string.Join(", ", result.Errors.Select(e => e.Description)));

                    Console.WriteLine($"❌ Failed: {entry.Username} -> {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"ℹ️  User {entry.Username} already exists, skipping...");
            }
        }
    }
}
