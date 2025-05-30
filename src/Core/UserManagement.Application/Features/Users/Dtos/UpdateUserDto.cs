using FluentValidation;

namespace UserManagement.Application.Features.Users.Dtos;

public record UpdateUserDto(Guid Id, string UserName, string Email, string Role, string ModifiedBy);

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.UserName)
            .NotEmpty().MinimumLength(4)
            .WithMessage("Username must be at least 4 characters long.");

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress()
            .WithMessage("A valid email is required.");

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role => role == "Admin" || role == "Manager")
            .WithMessage("Role must be Admin or Manager.");
    }
}
