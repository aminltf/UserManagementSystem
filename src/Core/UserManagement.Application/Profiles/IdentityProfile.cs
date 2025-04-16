using AutoMapper;
using UserManagement.Application.Features.Users.Dtos;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Profiles;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<ApplicationUser, UserDto>();
    }
}
