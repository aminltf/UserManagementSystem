using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.UpdateUser;

        // 1. Get user from DB
        var user = await _userManager.FindByIdAsync(dto.Id.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with ID '{dto.Id}' not found.");

        // 2. Check for duplicate username
        if (user.UserName != dto.UserName)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(dto.UserName);
            if (userWithSameUserName != null)
                throw new InvalidOperationException($"Username '{dto.UserName}' is already taken.");
        }

        // 3. Check for duplicate email
        if (user.Email != dto.Email)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (userWithSameEmail != null)
                throw new InvalidOperationException($"Email '{dto.Email}' is already taken.");
        }

        // 4. Update properties
        user.UserName = dto.UserName;
        user.Email = dto.Email;
        user.ModifiedBy = dto.ModifiedBy;
        user.ModifiedAt = DateTime.UtcNow;

        // 5. Update user
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ApplicationException($"Failed to update user: {errors}");
        }

        // 6. Update role if needed
        var currentRoles = await _userManager.GetRolesAsync(user);
        var currentRole = currentRoles.FirstOrDefault();
        if (!string.Equals(currentRole, dto.Role, StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrWhiteSpace(currentRole))
                await _userManager.RemoveFromRoleAsync(user, currentRole);

            var roleResult = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new ApplicationException($"Failed to update role to '{dto.Role}': {errors}");
            }
        }

        return true;
    }
}
