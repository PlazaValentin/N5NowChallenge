using AutoMapper;
using MediatR;
using Microsoft.Extensions.Hosting;
using N5NowChallenge.Application.Commands.Base;
using N5NowChallenge.Application.DTOs;
using N5NowChallenge.Application.Kafka;
using N5NowChallenge.Application.Queries.Base;
using N5NowChallenge.Domain.Entities;
using N5NowChallenge.Infrastructure.Repositories.Base;
using Nest;

namespace N5NowChallenge.Application.Queries.GetPermissions;

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, GetAllPermissionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IProducerService _producerService;
    private readonly IHostEnvironment _hostEnvironment;
    public GetAllPermissionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IProducerService producerService, IHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _producerService = producerService;
        _hostEnvironment = hostEnvironment;
    }
    public async Task<GetAllPermissionResponse?> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var searchRequest = new SearchRequest<Permission>
        {
            Query = new MatchAllQuery()
        };

        var permissions = await _unitOfWork.PermissionRepository.SearchDocumentsInIndexAsync(searchRequest, cancellationToken);

        if (_hostEnvironment.IsDevelopment())
            await _producerService.ProduceMessageAsync(new ProducerDTO(Guid.NewGuid(), "get"));

        return GetAllPermissionResponse.Success( _mapper.Map<IEnumerable<PermissionDTO>>(permissions).OrderBy(p => p.Id) );
    }
}
