using Microsoft.EntityFrameworkCore.Infrastructure;

namespace N5NowChallenge.Infrastructure.Repositories.Base;

public interface IUnitOfWork
{
    IPermissionRepository PermissionRepository { get; }
    Task<bool> SaveAsync(CancellationToken cancellationToken = default);
}