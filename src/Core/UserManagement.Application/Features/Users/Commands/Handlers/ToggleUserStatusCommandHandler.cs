using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ToggleUserStatusCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Retrieve user by ID
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with ID '{request.Id}' was not found.");

        // Step 2: Ensure the user is not deleted (optional)
        if (user.IsDeleted)
            throw new InvalidOperationException("Deleted user cannot be activated or deactivated.");

        // Step 3: Toggle status based on current IsActive value
        if (user.IsActive)
            // Deactivate the user
            user.Deactivate(request.ModifiedBy);
        else
            // Activate the user
            user.Activate(request.ModifiedBy);

        // Step 4: Update user via UserManager
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ApplicationException($"Toggling user status failed: {errors}");
        }

        // Step 5: Return success
        return true;
    }
}
