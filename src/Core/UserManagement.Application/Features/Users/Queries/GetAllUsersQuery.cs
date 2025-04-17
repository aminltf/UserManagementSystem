using MediatR;
using UserManagement.Application.Common.Models.Filtering;
using UserManagement.Application.Common.Models.Pagination;
using UserManagement.Application.Common.Models.Sorting;
using UserManagement.Application.Features.Users.Dtos;

namespace UserManagement.Application.Features.Users.Queries;

public class GetAllUsersQuery : IRequest<PageResponse<UserDto>>
{
    public PageRequest Pagination { get; set; } = new();
    public SortOptions Sort { get; set; } = new();
    public UserFilter Filter { get; set; } = new();
    public string? SearchTerm { get; set; }
}
