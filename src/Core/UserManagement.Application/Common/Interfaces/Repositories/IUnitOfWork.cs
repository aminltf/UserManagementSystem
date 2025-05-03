namespace UserManagement.Application.Common.Interfaces.Repositories;

public interface IUnitOfWork : IAsyncDisposable
{
    IUserQueryRepository UserQuery { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
