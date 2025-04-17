using FluentValidation;
using MediatR;

namespace UserManagement.Application.Features.Users.Commands;

public record ToggleUserStatusCommand(Guid Id, string ModifiedBy) : IRequest<Unit>;

public class ToggleUserStatusCommandValidator : AbstractValidator<ToggleUserStatusCommand>
{
    public ToggleUserStatusCommandValidator()
    {
        
    }
}
