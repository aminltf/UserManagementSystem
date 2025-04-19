using FluentValidation;

namespace UserManagement.Application.Features.Users.Dtos;

public record UserDto(Guid Id, string UserName, string Email, string Role, bool IsActive);

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {

    }
}
