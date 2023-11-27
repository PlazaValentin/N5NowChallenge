using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace N5NowChallenge.Infrastructure.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private readonly IPermissionRepository _permissionRepository;

    public UnitOfWork(
        AppDbContext dbContext,
        IPermissionRepository permissionRepository)
    {
        _dbContext = dbContext;
        _permissionRepository = permissionRepository;
    }

    public IPermissionRepository PermissionRepository => _permissionRepository;

    public async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
}