using MediatR;

namespace N5NowChallenge.Application.Commands.CreatePermission;

public record CreatePermissionCommand(string FirstNameEmployee, string LastNameEmployee, int PermissionTypeId, DateTime DatePermission) : IRequest<CreatePermissionResponse>;