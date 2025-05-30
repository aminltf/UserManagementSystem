using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Features.Users.Dtos;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Features.Users.Queries.Handlers;

public class GetUserStatusByIdQueryHandler : IRequestHandler<GetUserStatusByIdQuery, UserStatusDto>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserStatusByIdQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserStatusDto> Handle(GetUserStatusByIdQuery request, CancellationToken cancellationToken)
    {
        // Step 1: Find user by ID
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with ID '{request.Id}' was not found.");

        // Step 2: Project to DTO
        return new UserStatusDto(
            IsActive: user.IsActive,
            IsDeleted: user.IsDeleted
        );
    }
}
