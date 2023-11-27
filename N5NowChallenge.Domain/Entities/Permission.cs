using N5NowChallenge.Domain.Entities.Base;

namespace N5NowChallenge.Domain.Entities;
public class Permission : Entity
{
    public string FirstNameEmployee { get; set; } = string.Empty;
    public string LastNameEmployee { get; set; } = string.Empty;
    public int PermissionTypeId { get; set; }
    public DateTime DatePermission { get; set; }

    public PermissionType? PermissionType { get; set; }
}
