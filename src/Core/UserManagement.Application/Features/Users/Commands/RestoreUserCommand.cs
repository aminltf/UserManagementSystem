using FluentValidation;
using MediatR;

namespace UserManagement.Application.Features.Users.Commands;

public record RestoreUserCommand(Guid Id, string ModifiedBy) : IRequest<Unit>;

public class RestoreUserCommandValidator : AbstractValidator<RestoreUserCommand>
{
    public RestoreUserCommandValidator()
    {
        
    }
}
