using N5NowChallenge.Application.Commands.Base;
using N5NowChallenge.Application.DTOs;

namespace N5NowChallenge.Application.Commands.UpdatePermission;

public record UpdatePermissionResponse : CommandResponse<PermissionDTO>;