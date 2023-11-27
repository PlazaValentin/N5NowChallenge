using AutoMapper;
using MediatR;
using Microsoft.Extensions.Hosting;
using N5NowChallenge.Application.Commands.CreatePermission;
using N5NowChallenge.Application.DTOs;
using N5NowChallenge.Application.Kafka;
using N5NowChallenge.Domain.Entities;
using N5NowChallenge.Infrastructure.Repositories.Base;
using Nest;
using System.Net;

namespace N5NowChallenge.Application.Commands.UpdatePermission;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, UpdatePermissionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IProducerService _producerService;
    private readonly IHostEnvironment _hostEnvironment;
    public UpdatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IProducerService producerService, IHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _producerService = producerService;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<UpdatePermissionResponse?> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        var existingPermission = await _unitOfWork.PermissionRepository.FindAsync(request.PermissionId, cancellationToken);

        if (existingPermission is null)
            return new UpdatePermissionResponse
            {
                ErrorMessage = "The requested entity was not found.",
                StatusCode = HttpStatusCode.BadRequest
            };

        var permission = _mapper.Map(request, existingPermission);

        _unitOfWork.PermissionRepository.Update(permission);
        var saved = await _unitOfWork.SaveAsync(cancellationToken);

        if (!saved)
            return new UpdatePermissionResponse
            {
                ErrorMessage = "An error occurred while processing the request. Please report to support and try again later.",
                StatusCode = HttpStatusCode.InternalServerError
            };

        var searchRequest = new SearchRequest<Permission>
        {
            Query = new TermQuery
            {
                Field = "id",
                Value = permission.Id
            }
        };
        var permissionElastic = await _unitOfWork.PermissionRepository.SearchDocumentsInIndexAsync(searchRequest);
        if(permissionElastic is not null)
        {
            var updateRequest = new UpdateRequest<Permission, Permission>(request.PermissionId)
            {
                Doc = permission,
                DocAsUpsert = true
            };

            await _unitOfWork.PermissionRepository.UpdateDocumentInIndexAsync(updateRequest, cancellationToken);
        }

        if (_hostEnvironment.IsDevelopment())
            await _producerService.ProduceMessageAsync(new ProducerDTO(Guid.NewGuid(), "modify"));

        return new UpdatePermissionResponse { Result = _mapper.Map<PermissionDTO>(permission) };
    }
}
