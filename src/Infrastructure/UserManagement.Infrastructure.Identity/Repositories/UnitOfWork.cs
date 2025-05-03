using UserManagement.Application.Common.Interfaces.Repositories;
using UserManagement.Infrastructure.Identity.Contexts;

namespace UserManagement.Infrastructure.Identity.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IdentityContext _context;

    public UnitOfWork(IdentityContext context, IUserQueryRepository repository)
    {
        _context = context;
        UserQuery = repository;
    }

    public IUserQueryRepository UserQuery { get; }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
