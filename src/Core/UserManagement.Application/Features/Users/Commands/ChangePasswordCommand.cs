using FluentValidation;
using MediatR;

namespace UserManagement.Application.Features.Users.Commands;

public record ChangePasswordCommand(Guid UserId, string CurrentPassword, string NewPassword, string ModifiedBy) : IRequest<Unit>;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        
    }
}
