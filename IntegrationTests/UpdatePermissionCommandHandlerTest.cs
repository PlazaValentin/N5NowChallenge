using Application.IntegrationTests.Base;
using AutoMapper;
using N5NowChallenge.Application.Kafka;
using N5NowChallenge.Infrastructure.Repositories.Base;
using N5NowChallenge.Infrastructure;
using N5NowChallenge.Application.Commands.UpdatePermission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using N5NowChallenge.Application.DTOs;
using FluentAssertions;

namespace Application.IntegrationTests;

public class UpdatePermissionCommandHandlerTest
{
    private readonly IntegrationTestConfiguration _integrationTestConfiguration;
    private readonly AppDbContext _appDbContext;
    private readonly UnitOfWork _unitOfWork;
    private readonly UpdatePermissionCommandHandler _handler;
    private readonly ProducerService _producerService;
    private readonly Mapper _mapper;
    private readonly IHostEnvironment _hostEnvironment;

    public UpdatePermissionCommandHandlerTest()
    {
        _integrationTestConfiguration = new IntegrationTestConfiguration();
        _mapper = _integrationTestConfiguration.Mapper;
        _appDbContext = _integrationTestConfiguration.AppDbContext;
        _unitOfWork = _integrationTestConfiguration.UnitOfWork;
        _producerService = _integrationTestConfiguration.ProducerService;
        _hostEnvironment = _integrationTestConfiguration.HostEnvironment;
        _handler = new UpdatePermissionCommandHandler(_unitOfWork, _mapper, _producerService, _hostEnvironment);
    }

    [Fact]
    public async Task UpdatePermission_Should_ReturnValidPermission()
    {
        var command = new UpdatePermissionCommand(5, "Valentin", "Plaza", 1, DateTime.Now);

        var response = await _handler.Handle(command, default);

        var permission = await _appDbContext.Permissions.SingleOrDefaultAsync(x => x.Id == command.PermissionId);
        var permissionDTO = _mapper.Map<PermissionDTO>(permission);

        response.Result.Should().BeEquivalentTo(permissionDTO);
    }
}
