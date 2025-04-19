#nullable disable

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Features.Users.Dtos;
using UserManagement.Domain.Entities.Identity;
using UserManagement.Shared.Kernel.Enums;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser(
                userName: request.UserName,
                email: request.Email,
                passwordHash: "",
                role: Enum.Parse<UserRole>(request.Role, true)
            )
        {
            CreatedBy = request.CreatedBy
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, request.Role);

        return _mapper.Map<UserDto>(user);
    }
}
