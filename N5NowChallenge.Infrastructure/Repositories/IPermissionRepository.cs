using N5NowChallenge.Domain.Entities;
using N5NowChallenge.Infrastructure.Repositories.Base;

namespace N5NowChallenge.Infrastructure.Repositories;

public interface IPermissionRepository : IElasticSearchRepository<Permission>
{
}
