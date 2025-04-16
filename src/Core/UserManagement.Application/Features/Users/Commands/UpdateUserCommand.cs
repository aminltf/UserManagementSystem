using FluentValidation;
using MediatR;
using UserManagement.Application.Features.Users.Dtos;

namespace UserManagement.Application.Features.Users.Commands;

public record UpdateUserCommand(Guid Id, string Role, string ModifiedBy) : IRequest<UserDto>;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Role).NotEmpty().Must(r => r == "Admin" || r == "Manager");
    }
}
