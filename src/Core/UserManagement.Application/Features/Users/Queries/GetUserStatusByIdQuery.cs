using MediatR;
using UserManagement.Application.Features.Users.Dtos;

namespace UserManagement.Application.Features.Users.Queries;

public record GetUserStatusByIdQuery(Guid Id) : IRequest<UserStatusDto>;
