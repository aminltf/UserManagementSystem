using FluentValidation;

namespace UserManagement.Application.Features.Users.Dtos;

public record CreateUserDto(string UserName, string Email, string Password, string Role, string CreatedBy);

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MinimumLength(4);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Role).NotEmpty().Must(role =>
            role == "Admin" || role == "Manager").WithMessage("Role must be Admin or Manager.");
    }
}
