namespace UserManagement.Application.Common.Models.Sorting;

public class SortOptions
{
    public string? OrderBy { get; set; } = "UserName";
    public string? Direction { get; set; } = "asc"; // asc or desc
}
