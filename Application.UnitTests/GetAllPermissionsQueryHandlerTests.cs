using AutoMapper;
using Moq;
using N5NowChallenge.Application.DTOs;
using N5NowChallenge.Application.Kafka;
using N5NowChallenge.Domain.Entities;
using N5NowChallenge.Infrastructure.Repositories.Base;
using N5NowChallenge.Infrastructure.Repositories;
using Nest;
using N5NowChallenge.Application.Mappers;
using Microsoft.Extensions.Hosting;
using N5NowChallenge.Application.Queries.GetPermissions;

namespace Application.UnitTests;

public class GetAllPermissionsQueryHandlerTests
{
    private readonly Mock<IPermissionRepository> _permissionRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProducerService> _producerServiceMock = new();

    [Fact]
    public async Task Handle_ShouldReturnMappedPermissionsAndProduceMessage()
    {
        // Arrange
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new PermissionProfile()));
        var mapperMock = new Mapper(mapperConfig);
        var hostEnvironment = new Mock<IHostEnvironment>();
        var handler = new GetAllPermissionsQueryHandler(_unitOfWork.Object, mapperMock, _producerServiceMock.Object, hostEnvironment.Object);

        var permissions = GetPermissionList();
        var expectedResponse = GetPermissionDTOList();

        _unitOfWork.Setup(uow => uow.PermissionRepository)
                  .Returns(_permissionRepository.Object);

        _permissionRepository.Setup(r => r.SearchDocumentsInIndexAsync(It.IsAny<SearchRequest<Permission>>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(permissions);
        hostEnvironment.Setup(h => h.EnvironmentName).Returns("Development");


        // Act
        var result = await handler.Handle(new GetAllPermissionsQuery(), CancellationToken.None);

        // Assert
        result.Equals(expectedResponse);
        _permissionRepository.Verify(r => r.SearchDocumentsInIndexAsync(It.IsAny<SearchRequest<Permission>>(), It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(p => p.ProduceMessageAsync(It.IsAny<ProducerDTO>()), Times.Once);
    }

    private static List<PermissionDTO> GetPermissionDTOList()
    {
        return new List<PermissionDTO>()
        {
            new PermissionDTO {
                Id = 1,
                FirstNameEmployee = "Valentin",
                LastNameEmployee = "Plaza",
                PermissionTypeId = 1,
                DatePermission = new DateTime(2023, 11, 25)
            },
            new PermissionDTO {
                Id = 2,
                FirstNameEmployee = "Pedro",
                LastNameEmployee = "Plaza",
                PermissionTypeId = 1,
                DatePermission = new DateTime(2023, 11, 25)
            },
            new PermissionDTO {
                Id = 3,
                FirstNameEmployee = "Felipe",
                LastNameEmployee = "Plaza",
                PermissionTypeId = 1,
                DatePermission = new DateTime(2023, 11, 25)
            },
        };
    }
    private static List<Permission> GetPermissionList()
    {
        return new List<Permission>()
        {
            new Permission {
                Id = 1,
                FirstNameEmployee = "Valentin",
                LastNameEmployee = "Plaza",
                PermissionTypeId = 1,
                DatePermission = new DateTime(2023, 11, 25)
            },
            new Permission {
                Id = 2,
                FirstNameEmployee = "Pedro",
                LastNameEmployee = "Plaza",
                PermissionTypeId = 1,
                DatePermission = new DateTime(2023, 11, 25)
            },
            new Permission {
                Id = 3,
                FirstNameEmployee = "Felipe",
                LastNameEmployee = "Plaza",
                PermissionTypeId = 1,
                DatePermission = new DateTime(2023, 11, 25)
            },
        };
    }
}