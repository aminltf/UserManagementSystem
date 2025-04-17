#nullable disable

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class RestoreUserCommandHandler : IRequestHandler<RestoreUserCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RestoreUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == request.Id && u.IsDeleted, cancellationToken);

        if (user == null)
            throw new KeyNotFoundException("User not found or not deleted.");

        user.Restore(request.ModifiedBy);
        await _userManager.UpdateAsync(user);

        return Unit.Value;
    }
}
