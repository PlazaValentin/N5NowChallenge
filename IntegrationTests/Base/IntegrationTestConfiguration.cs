using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N5NowChallenge.Application.Kafka;
using N5NowChallenge.Infrastructure.Repositories.Base;
using N5NowChallenge.Infrastructure.Repositories;
using N5NowChallenge.Infrastructure;
using Nest;
using Confluent.Kafka;
using N5NowChallenge.Infrastructure.DependencyInjection;
using N5NowChallenge.Application.Mappers;
using Microsoft.Extensions.Hosting.Internal;

namespace Application.IntegrationTests.Base
{
    public class IntegrationTestConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly Mapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly ElasticClient _elasticClient;
        private readonly PermissionRepository _permissionRepository;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProducerService _producerService;
        private readonly IHostEnvironment _hostEnvironment;

        public IntegrationTestConfiguration()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../../..", "N5NowChallenge.Api"));
                    config.AddJsonFile($"appsettings.Development.json");
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IHostEnvironment>(_ => new HostingEnvironment { EnvironmentName = "Development" });
                })
                .Build();

            _hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();

            _configuration = host.Services.GetRequiredService<IConfiguration>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new PermissionProfile())));

            var serviceProvider = BuildServiceProvider();

            _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
            _elasticClient = CreateElasticClient();
            _permissionRepository = new PermissionRepository(_elasticClient, _appDbContext);
            _unitOfWork = new UnitOfWork(_appDbContext, _permissionRepository);
            _producerService = CreateProducerService();
        }

        public IConfiguration Configuration => _configuration;
        public Mapper Mapper => _mapper;
        public AppDbContext AppDbContext => _appDbContext;
        public ElasticClient ElasticClient => _elasticClient;
        public PermissionRepository PermissionRepository => _permissionRepository;
        public UnitOfWork UnitOfWork => _unitOfWork;
        public ProducerService ProducerService => _producerService;
        public IHostEnvironment HostEnvironment => _hostEnvironment;

        private ProducerService CreateProducerService()
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:Uri"]
            };
            var kafkaTopic = _configuration["Kafka:Topic"];

            return new ProducerService(producerConfig, kafkaTopic);
        }

        private ElasticClient CreateElasticClient()
        {
            var urlElasticSearch = _configuration["ElasticSettings:Uri"];
            var defaultIndex = _configuration["ElasticSettings:index"];

            var elasticSearchSettings = new ConnectionSettings(new Uri(urlElasticSearch)).PrettyJson()
                .DefaultIndex(defaultIndex);

            ElasticConfiguration.AddDefaultMapping(elasticSearchSettings);

            var elasticClient = new ElasticClient(elasticSearchSettings);
            ElasticConfiguration.CreateIndex(elasticClient, defaultIndex);

            return elasticClient;
        }

        private IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddInfrastructure(_configuration);

            return services.BuildServiceProvider();
        }
    }
}
