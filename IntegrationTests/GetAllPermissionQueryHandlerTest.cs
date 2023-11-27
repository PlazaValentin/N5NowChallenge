using Application.IntegrationTests.Base;
using AutoMapper;
using N5NowChallenge.Application.Kafka;
using N5NowChallenge.Infrastructure.Repositories.Base;
using N5NowChallenge.Infrastructure;
using Microsoft.EntityFrameworkCore;
using N5NowChallenge.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using N5NowChallenge.Application.Queries.GetPermissions;
using N5NowChallenge.Application.DTOs;

namespace Application.IntegrationTests;

[Collection("CreatePermissionCollection")]
public class GetAllPermissionQueryHandlerTest
{
    private readonly IntegrationTestConfiguration _integrationTestConfiguration;
    private readonly AppDbContext _appDbContext;
    private readonly UnitOfWork _unitOfWork;
    private readonly GetAllPermissionsQueryHandler _handler;
    private readonly ProducerService _producerService;
    private readonly Mapper _mapper;
    private readonly IHostEnvironment _hostEnvironment;

    public GetAllPermissionQueryHandlerTest()
    {
        _integrationTestConfiguration = new IntegrationTestConfiguration();
        _mapper = _integrationTestConfiguration.Mapper;
        _appDbContext = _integrationTestConfiguration.AppDbContext;
        _unitOfWork = _integrationTestConfiguration.UnitOfWork;
        _producerService = _integrationTestConfiguration.ProducerService;
        _hostEnvironment = _integrationTestConfiguration.HostEnvironment;
        _handler = new GetAllPermissionsQueryHandler(_unitOfWork, _mapper, _producerService, _hostEnvironment);
    }

    [Fact]
    public async Task GetAllPermission_Should_ReturnValidPermissionList()
    {
        var query = new GetAllPermissionsQuery();

        var permissionsDTO = await _handler.Handle(query, default);

        var permissions = await _appDbContext.Permissions.ToListAsync();

        Assert.NotNull(permissionsDTO.Result);
        Assert.Equal(permissions.Count, permissionsDTO.Result.ToList().Count);

        var expected = _mapper.Map<List<PermissionDTO>>(permissions);
        permissionsDTO.Result.Should().BeEquivalentTo(expected, options => options
            .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMinutes(1))).WhenTypeIs<DateTime>());
    }
}
