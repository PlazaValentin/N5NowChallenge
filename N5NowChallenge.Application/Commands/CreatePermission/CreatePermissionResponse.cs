using N5NowChallenge.Application.Commands.Base;
using N5NowChallenge.Application.DTOs;

namespace N5NowChallenge.Application.Commands.CreatePermission;

public record CreatePermissionResponse : CommandResponse<PermissionDTO>;
