namespace UserManagement.Application.Common.Interfaces;

public interface IPagedQuery
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
}
