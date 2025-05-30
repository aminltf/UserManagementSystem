using FluentValidation;

namespace UserManagement.Application.Features.Users.Dtos;

public record ChangePasswordDto(Guid UserId, string CurrentPassword, string NewPassword, string ModifiedBy);

public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().MinimumLength(6).WithMessage("Current password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().MinimumLength(6).WithMessage("New password must be at least 6 characters.")
            .NotEqual(x => x.CurrentPassword).WithMessage("New password must be different from the current password.");
    }
}
