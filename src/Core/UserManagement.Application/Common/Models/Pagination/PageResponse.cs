namespace UserManagement.Application.Common.Models.Pagination;

public class PageResponse
{

}

public class PageResponse<T> : PageResponse
{
    public List<T> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
