using System.Reflection;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N5NowChallenge.Application.Kafka;

namespace N5NowChallenge.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviours.LoggingBehavior<,>));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:Uri"]
        };
        var kafkaTopic = configuration["Kafka:Topic"];

        services.AddScoped<IProducerService>(provider => new ProducerService(producerConfig, kafkaTopic));

        return services;
    }
}