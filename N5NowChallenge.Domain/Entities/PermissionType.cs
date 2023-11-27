using N5NowChallenge.Domain.Entities.Base;

namespace N5NowChallenge.Domain.Entities;

public class PermissionType : Entity
{
    public string Description { get; set; } = string.Empty;
    public List<Permission>? Permissions { get; set; }
}