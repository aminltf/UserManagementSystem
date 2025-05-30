using MediatR;

namespace UserManagement.Application.Features.Users.Commands;

public record ToggleUserStatusCommand(Guid Id, string ModifiedBy) : IRequest<bool>;
