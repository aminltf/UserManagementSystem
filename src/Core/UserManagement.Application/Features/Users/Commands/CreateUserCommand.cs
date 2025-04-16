using FluentValidation;
using MediatR;
using UserManagement.Application.Features.Users.Dtos;

namespace UserManagement.Application.Features.Users.Commands;

public record CreateUserCommand(string UserName, string Password, string Role, string CreatedBy) : IRequest<UserDto>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MinimumLength(4);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Role).NotEmpty().Must(role =>
            role == "Admin" || role == "Manager").WithMessage("Role must be Admin or Manager");
    }
}
