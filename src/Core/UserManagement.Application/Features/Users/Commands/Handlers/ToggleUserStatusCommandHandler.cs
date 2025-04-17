#nullable disable

using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ToggleUserStatusCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString())
                   ?? throw new Exception("User not found");

        if (user.IsActive)
            user.Deactivate(request.ModifiedBy);
        else
            user.Activate(request.ModifiedBy);

        await _userManager.UpdateAsync(user);

        return Unit.Value;
    }
}
