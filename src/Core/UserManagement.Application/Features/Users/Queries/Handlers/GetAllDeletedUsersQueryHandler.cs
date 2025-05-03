#nullable disable

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Common.Interfaces.Repositories;
using UserManagement.Application.Common.Models.Pagination;
using UserManagement.Application.Features.Users.Dtos;

namespace UserManagement.Application.Features.Users.Queries.Handlers;

public class GetAllDeletedUsersQueryHandler : IRequestHandler<GetAllDeletedUsersQuery, PageResponse<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllDeletedUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PageResponse<UserDto>> Handle(GetAllDeletedUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.UserQuery.GetAll(includeDeleted: true)
            .Where(x => x.IsDeleted)
            .AsQueryable();

        // Filtering
        if (request.Filter.Role is not null)
            query = query.Where(x => x.Role.ToString() == request.Filter.Role);

        if (request.Filter.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.Filter.IsActive);

        // Search
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(x =>
                x.UserName!.Contains(request.SearchTerm) ||
                x.Email!.Contains(request.SearchTerm));

        // Sorting
        if (request.Sort.Direction == "desc")
            query = request.Sort.OrderBy switch
            {
                "Email" => query.OrderByDescending(x => x.Email),
                _ => query.OrderByDescending(x => x.UserName),
            };
        else
            query = request.Sort.OrderBy switch
            {
                "Email" => query.OrderBy(x => x.Email),
                _ => query.OrderBy(x => x.UserName),
            };

        // Paging
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Pagination.PageNumber - 1) * request.Pagination.PageSize)
            .Take(request.Pagination.PageSize)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResponse<UserDto>
        {
            Items = items,
            TotalItems = totalCount,
            PageNumber = request.Pagination.PageNumber,
            PageSize = request.Pagination.PageSize
        };
    }
}
