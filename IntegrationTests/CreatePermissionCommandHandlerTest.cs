using Application.IntegrationTests.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using N5NowChallenge.Application.Commands.CreatePermission;
using N5NowChallenge.Application.Kafka;
using N5NowChallenge.Domain.Entities;
using N5NowChallenge.Infrastructure;
using N5NowChallenge.Infrastructure.Repositories.Base;
using Nest;

namespace Application.IntegrationTests;

[Collection("CreatePermissionCollection")]
public class CreatePermissionCommandHandlerTest
{
    private readonly IntegrationTestConfiguration _integrationTestConfiguration;
    private readonly AppDbContext _appDbContext;
    private readonly UnitOfWork _unitOfWork;
    private readonly CreatePermissionCommandHandler _handler;
    private readonly ProducerService _producerService;
    private readonly Mapper _mapper;
    private readonly IHostEnvironment _hostEnvironment;

    public CreatePermissionCommandHandlerTest()
    {
        _integrationTestConfiguration = new IntegrationTestConfiguration();
        _mapper = _integrationTestConfiguration.Mapper;
        _appDbContext = _integrationTestConfiguration.AppDbContext;
        _unitOfWork = _integrationTestConfiguration.UnitOfWork;
        _producerService = _integrationTestConfiguration.ProducerService;
        _hostEnvironment = _integrationTestConfiguration.HostEnvironment;
        _handler = new CreatePermissionCommandHandler(_unitOfWork, _mapper, _producerService, _hostEnvironment);
    }

    [Fact]
    public async Task CreatePermission_Should_ReturnValidPermission()
    {
        var command = new CreatePermissionCommand("Valentin", "Plaza", 1, DateTime.Now);

        var permissionDTO = await _handler.Handle(command, default);

        var permission = await _appDbContext.Permissions.SingleOrDefaultAsync(x => x.Id == permissionDTO.Result.Id);

        var searchRequest = new SearchRequest<Permission>
        {
            Query = new TermQuery
            {
                Field = "id",
                Value = permissionDTO.Result.Id
            }
        };
        var permissionElastic = await _unitOfWork.PermissionRepository.SearchDocumentsInIndexAsync(searchRequest);

        Assert.NotNull(permission);
        Assert.True(permission.Id.Equals(permissionDTO.Result.Id));
        permission.Equals(permissionElastic);
    }
}
