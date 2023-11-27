using N5NowChallenge.Domain.Entities;
using N5NowChallenge.Infrastructure.Repositories.Base;
using Nest;

namespace N5NowChallenge.Infrastructure.Repositories
{
    public class PermissionRepository : ElasticSearchRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IElasticClient client, AppDbContext dbContext) : base (client, dbContext) {}
    }
}
