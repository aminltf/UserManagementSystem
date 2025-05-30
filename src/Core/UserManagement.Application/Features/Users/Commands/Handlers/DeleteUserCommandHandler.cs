using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Retrieve the user by ID
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with ID '{request.Id}' was not found.");

        // Step 2: Ensure the user has not already been deleted
        if (user.IsDeleted)
            throw new InvalidOperationException("User has already been soft deleted.");

        // Step 3: Perform soft delete (set IsDeleted, DeletedBy, DeletedAt)
        user.SoftDelete(request.DeletedBy);

        // Step 4: Save changes via UserManager
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ApplicationException($"User deletion failed: {errors}");
        }

        // Step 5: Return success
        return true;
    }
}
