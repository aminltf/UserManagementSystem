using UserManagement.Domain.Entities.Identity;

namespace UserManagement.Application.Common.Interfaces.Repositories;

public interface IUserQueryRepository
{
    IQueryable<ApplicationUser> GetAll(bool includeDeleted = false);
}
