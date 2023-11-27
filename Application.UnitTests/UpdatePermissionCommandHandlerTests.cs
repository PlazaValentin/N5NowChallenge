using AutoMapper;
using Moq;
using N5NowChallenge.Application.Commands.UpdatePermission;
using N5NowChallenge.Application.Kafka;
using N5NowChallenge.Domain.Entities;
using N5NowChallenge.Infrastructure.Repositories.Base;
using N5NowChallenge.Infrastructure.Repositories;
using N5NowChallenge.Application.Mappers;
using Nest;
using N5NowChallenge.Application.DTOs;
using Microsoft.Extensions.Hosting;

namespace Application.UnitTests;

public class UpdatePermissionCommandHandlerTests
{
    private readonly Mock<IPermissionRepository> _permissionRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProducerService> _producerServiceMock = new();

    [Fact]
    public async Task Handle_ShouldUpdatePermissionAndProduceMessage()
    {
        // Arrange
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new PermissionProfile()));
        var mapperMock = new Mapper(mapperConfig);
        var hostEnvironment = new Mock<IHostEnvironment>();
        var handler = new UpdatePermissionCommandHandler(_unitOfWork.Object, mapperMock, _producerServiceMock.Object, hostEnvironment.Object);

        var existingPermission = GetPermission();
        var updatePermissionRequest = GetUpdatePermissionRequest();
        var expectedResponse = GetPermissionDTO();


        _permissionRepository.Setup(r => r.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(existingPermission);
        _unitOfWork.Setup(uow => uow.PermissionRepository)
                  .Returns(_permissionRepository.Object);
        _unitOfWork.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true);
        hostEnvironment.Setup(h => h.EnvironmentName).Returns("Development");

        // Act
        var result = await handler.Handle(updatePermissionRequest, CancellationToken.None);

        result.Equals(expectedResponse);
        _permissionRepository.Verify(r => r.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(uow => uow.PermissionRepository.Update(It.IsAny<Permission>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(uow => uow.PermissionRepository.UpdateDocumentInIndexAsync(It.IsAny<UpdateRequest<Permission, Permission>>(), It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceMessageAsync(It.IsAny<ProducerDTO>()), Times.Once);
    }

    private static UpdatePermissionCommand GetUpdatePermissionRequest()
    {
        return new UpdatePermissionCommand(1, "Valentin", "Plaza", 1, new DateTime(2023, 11, 25));
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
}
