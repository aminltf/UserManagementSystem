using MediatR;

namespace UserManagement.Application.Features.Users.Commands;

public record RestoreUserCommand(Guid Id, string ModifiedBy) : IRequest<bool>;
