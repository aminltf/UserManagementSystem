using UserManagement.Domain.Common.Interfaces;

namespace UserManagement.Domain.Common.Abstractions;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
