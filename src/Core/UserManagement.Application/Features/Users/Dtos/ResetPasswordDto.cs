using FluentValidation;

namespace UserManagement.Application.Features.Users.Dtos;

public record ResetPasswordDto(Guid UserId, string NewPassword, string ConfirmPassword, string ModifiedBy);

public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().MinimumLength(6).WithMessage("New password must be at least 6 characters.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
    }
}
