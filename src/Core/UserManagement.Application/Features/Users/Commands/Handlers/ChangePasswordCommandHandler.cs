﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var dto = request.ChangePassword;

        // 1. Find user
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with ID '{dto.UserId}' not found.");

        // 2. Change password
        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ApplicationException($"Password change failed: {errors}");
        }

        // 3. Update modified info (optional)
        user.ModifiedBy = dto.ModifiedBy;
        user.ModifiedAt = DateTime.UtcNow;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            throw new ApplicationException($"User update after password change failed: {errors}");
        }

        // 4. Return success
        return Unit.Value;
    }
}
