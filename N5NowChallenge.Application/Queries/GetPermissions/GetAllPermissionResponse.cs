using N5NowChallenge.Application.DTOs;
using N5NowChallenge.Application.Queries.Base;

namespace N5NowChallenge.Application.Queries.GetPermissions;

public record GetAllPermissionResponse : QueryResponse<IEnumerable<PermissionDTO>, GetAllPermissionResponse>;
