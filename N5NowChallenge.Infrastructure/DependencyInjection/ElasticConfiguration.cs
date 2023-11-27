using N5NowChallenge.Domain.Entities;
using Nest;

namespace N5NowChallenge.Infrastructure.DependencyInjection;
public class ElasticConfiguration
{
    public static void AddDefaultMapping(ConnectionSettings settings)
    {
        settings.DefaultMappingFor<Permission>(p =>
            p.Ignore(x => x.PermissionType)
        );
    }

    public static void CreateIndex(IElasticClient client, string indexName)
    {
        client.Indices.Create(indexName, i => i.Map<Permission>(x => x.AutoMap()));
    }
}
