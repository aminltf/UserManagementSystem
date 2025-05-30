using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CreateUser;

        // 1. Check for duplicate username or email (if needed)
        var existingUser = await _userManager.FindByNameAsync(dto.UserName);
        if (existingUser != null)
            throw new InvalidOperationException($"User with username '{dto.UserName}' already exists.");

        var existingEmail = await _userManager.FindByEmailAsync(dto.Email);
        if (existingEmail != null)
            throw new InvalidOperationException($"User with email '{dto.Email}' already exists.");

        // 2. Map DTO to ApplicationUser entity
        var user = _mapper.Map<ApplicationUser>(dto);
        user.UserName = dto.UserName;
        user.Email = dto.Email;
        user.CreatedBy = dto.CreatedBy;
        user.CreatedAt = DateTime.UtcNow;

        // 3. Create user
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ApplicationException($"User creation failed: {errors}");
        }

        // 4. Assign role
        if (!string.IsNullOrWhiteSpace(dto.Role))
        {
            var roleResult = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new ApplicationException($"Failed to assign role '{dto.Role}': {errors}");
            }
        }

        // 5. Return user Id
        return user.Id;
    }
}
