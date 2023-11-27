using MediatR;

namespace N5NowChallenge.Application.Commands.UpdatePermission;
public record UpdatePermissionCommand(int PermissionId, string FirstNameEmployee, string LastNameEmployee, int PermissionTypeId, DateTime DatePermission) : IRequest<UpdatePermissionResponse>;
