using MediatR;

namespace UserManagement.Application.Features.Users.Commands;

public record DeleteUserCommand(Guid Id, string DeletedBy) : IRequest<bool>;
