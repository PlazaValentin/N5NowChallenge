using MediatR;

namespace N5NowChallenge.Application.Queries.GetPermissions;

public record GetAllPermissionsQuery() : IRequest<GetAllPermissionResponse>;
