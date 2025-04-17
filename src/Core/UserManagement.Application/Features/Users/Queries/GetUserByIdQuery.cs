using FluentValidation;
using MediatR;
using UserManagement.Application.Features.Users.Dtos;

namespace UserManagement.Application.Features.Users.Queries;

public record GetUserByIdQuery(Guid Id, bool IncludeDeleted = false) : IRequest<UserDto>;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        
    }
}
