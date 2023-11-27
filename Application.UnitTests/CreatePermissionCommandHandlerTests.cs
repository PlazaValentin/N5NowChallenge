using AutoMapper;
using Microsoft.Extensions.Hosting;
using Moq;
using N5NowChallenge.Application.Commands.CreatePermission;
using N5NowChallenge.Application.DTOs;
using N5NowChallenge.Application.Kafka;
using N5NowChallenge.Application.Mappers;
using N5NowChallenge.Domain.Entities;
using N5NowChallenge.Infrastructure.Repositories.Base;

namespace Application.UnitTests;

public class CreatePermissionCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreatePermissionAndProduceMessage()
    {
        // Arrange
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new PermissionProfile()));
        var mapperMock = new Mapper(mapperConfig);
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var producerServiceMock = new Mock<IProducerService>();
        var hostEnvironmentMock = new Mock<IHostEnvironment>();
        hostEnvironmentMock.Setup(h => h.EnvironmentName).Returns("Development");

        var handler = new CreatePermissionCommandHandler(unitOfWorkMock.Object, mapperMock, producerServiceMock.Object, hostEnvironmentMock.Object);

        var request = GetCreatePermissionCommand();
        var responseRepository = GetPermission();
        var responseExpected = GetPermissionDTO();

        unitOfWorkMock.Setup(u => u.PermissionRepository.CreateAsync(It.IsAny<Permission>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(responseRepository);
        unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true);

        hostEnvironmentMock.Setup(h => h.EnvironmentName).Returns("Development");

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Equals(responseExpected);
        unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(u => u.PermissionRepository.AddDocumentToIndexAsync(It.IsAny<Permission>(), It.IsAny<CancellationToken>()), Times.Once);
        producerServiceMock.Verify(p => p.ProduceMessageAsync(It.IsAny<ProducerDTO>()), Times.Once);
    }

    private static CreatePermissionCommand GetCreatePermissionCommand()
    {
        return new CreatePermissionCommand("Valentin", "Plaza", 1, new DateTime(2023, 11, 25));
    }

    private static PermissionDTO GetPermissionDTO()
    {
        return new PermissionDTO()
        {
            Id = 1,
            FirstNameEmployee = "Valentin",
            LastNameEmployee = "Plaza",
            PermissionTypeId = 1,
            DatePermission = new DateTime(2023, 11, 25)
        };
    }

    private static Permission GetPermission()
    {
        return new Permission()
        {
            Id = 1,
            FirstNameEmployee = "Valentin",
            LastNameEmployee = "Plaza",
            PermissionTypeId = 1,
            DatePermission = new DateTime(2023, 11, 25)
        };
    }
}
