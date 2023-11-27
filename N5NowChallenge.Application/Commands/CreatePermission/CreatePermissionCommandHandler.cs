using AutoMapper;
using MediatR;
using Microsoft.Extensions.Hosting;
using N5NowChallenge.Application.Commands.CreatePermission;
using N5NowChallenge.Application.DTOs;
using N5NowChallenge.Application.Kafka;
using N5NowChallenge.Domain.Entities;
using N5NowChallenge.Infrastructure.Repositories.Base;
using System.Net;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, CreatePermissionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IProducerService _producerService;
    private readonly IHostEnvironment _hostEnvironment;

    public CreatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IProducerService producerService, IHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _producerService = producerService;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<CreatePermissionResponse?> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = _mapper.Map<Permission>(request);

        var permissionEntity = await _unitOfWork.PermissionRepository.CreateAsync(permission, cancellationToken);
        var saved = await _unitOfWork.SaveAsync(cancellationToken);

        if (!saved)
            return new CreatePermissionResponse
            {
                ErrorMessage = "An error occurred while processing the request. Please report to support and try again later.",
                StatusCode = HttpStatusCode.InternalServerError
            };

        await _unitOfWork.PermissionRepository.AddDocumentToIndexAsync(permissionEntity, cancellationToken);

        if (_hostEnvironment.IsDevelopment())
            await _producerService.ProduceMessageAsync(new ProducerDTO(Guid.NewGuid(), "request"));

        return new CreatePermissionResponse { Result = _mapper.Map<PermissionDTO>(permissionEntity) };
    }
}
