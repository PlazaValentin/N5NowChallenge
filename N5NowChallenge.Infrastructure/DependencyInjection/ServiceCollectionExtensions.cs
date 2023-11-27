using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N5NowChallenge.Infrastructure.Repositories.Base;
using Nest;

namespace N5NowChallenge.Infrastructure.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<UnitOfWork>()
            .AddClasses(classes => classes.AssignableTo(typeof(Repositories.Base.IRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }, ServiceLifetime.Scoped);

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var urlElasticSearch = configuration["ElasticSettings:Uri"];
        var defaultIndex = configuration["ElasticSettings:index"];

        var elasticSearchSettings = new ConnectionSettings(new Uri(urlElasticSearch)).PrettyJson()
            .DefaultIndex(defaultIndex);
        
        ElasticConfiguration.AddDefaultMapping(elasticSearchSettings);

        var elasticClient = new ElasticClient(elasticSearchSettings);

        services.AddSingleton<IElasticClient>(elasticClient);

        ElasticConfiguration.CreateIndex(elasticClient, defaultIndex);

        return services;
    }
}