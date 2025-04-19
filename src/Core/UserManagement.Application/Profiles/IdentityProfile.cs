using AutoMapper;
using UserManagement.Application.Features.Users.Commands;
using UserManagement.Application.Features.Users.Dtos;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Profiles;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<ApplicationUser, UserDto>();
        CreateMap<ChangePasswordCommand, ApplicationUser>();
        CreateMap<CreateUserCommand, ApplicationUser>();
        CreateMap<DeleteUserCommand, ApplicationUser>();
        CreateMap<RestoreUserCommand, ApplicationUser>();
        CreateMap<ToggleUserStatusCommand, ApplicationUser>();
        CreateMap<UpdateUserCommand, ApplicationUser>();
    }
}
