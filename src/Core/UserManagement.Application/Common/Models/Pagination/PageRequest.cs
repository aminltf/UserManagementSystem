using UserManagement.Application.Common.Interfaces;

namespace UserManagement.Application.Common.Models.Pagination;

public class PageRequest : IPagedQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
