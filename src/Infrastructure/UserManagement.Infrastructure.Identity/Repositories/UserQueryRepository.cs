#nullable disable

using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Common.Interfaces.Repositories;
using UserManagement.Domain.Entities.Identity;
using UserManagement.Infrastructure.Identity.Contexts;

namespace UserManagement.Infrastructure.Identity.Repositories;

public class UserQueryRepository : IUserQueryRepository
{
    private readonly IdentityContext _context;

    public UserQueryRepository(IdentityContext context)
    {
        _context = context;
    }

    public IQueryable<ApplicationUser> GetAll(bool includeDeleted = false)
    {
        if (includeDeleted)
            return _context.ApplicationUsers.IgnoreQueryFilters().Where(x => x.IsDeleted);

        return _context.ApplicationUsers.AsQueryable();
    }
}
