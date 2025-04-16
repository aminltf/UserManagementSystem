namespace UserManagement.Domain.Common.Interfaces;

public interface IAuditableEntity
{
    string? CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }
    string? ModifiedBy { get; set; }
    DateTime? ModifiedAt { get; set; }
    string? DeletedBy { get; set; }
    DateTime? DeletedAt { get; set; }
}
