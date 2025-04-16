#nullable disable

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Features.Users.Dtos;
using UserManagement.Domain.Entities.Identity;
using UserManagement.Shared.Kernel.Enums;

namespace UserManagement.Application.Features.Users.Commands.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString())
                   ?? throw new Exception("User not found");

        user.ModifiedBy = request.ModifiedBy;
        user.Role = Enum.Parse<UserRole>(request.Role, true);

        await _userManager.UpdateAsync(user);
        return _mapper.Map<UserDto>(user);
    }
}
