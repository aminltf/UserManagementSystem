using MediatR;
using UserManagement.Application.Features.Users.Dtos;

namespace UserManagement.Application.Features.Users.Commands;

public record ChangePasswordCommand(ChangePasswordDto ChangePassword) : IRequest<Unit>;
