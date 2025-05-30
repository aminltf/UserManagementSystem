using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class RestoreUserCommandHandler : IRequestHandler<RestoreUserCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RestoreUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Retrieve user by ID
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with ID '{request.Id}' was not found.");

        // Step 2: Check if user is already active (not deleted)
        if (!user.IsDeleted)
            throw new InvalidOperationException("User is not deleted and cannot be restored.");

        // Step 3: Perform restore logic (clears deletion flags)
        user.Restore(request.ModifiedBy);

        // Step 4: Save changes via UserManager
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ApplicationException($"User restore failed: {errors}");
        }

        // Step 5: Return success
        return true;
    }
}
