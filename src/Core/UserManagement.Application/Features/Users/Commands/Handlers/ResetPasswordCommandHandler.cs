using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var dto = request.ResetPassword;

        // 1. Find user
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with ID '{dto.UserId}' not found.");

        // 2. Generate reset token
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        // 3. Reset password
        var result = await _userManager.ResetPasswordAsync(user, resetToken, dto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ApplicationException($"Password reset failed: {errors}");
        }

        // 4. Update modified fields
        user.ModifiedBy = dto.ModifiedBy;
        user.ModifiedAt = DateTime.UtcNow;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            throw new ApplicationException($"Updating user after reset failed: {errors}");
        }

        return Unit.Value;
    }
}
