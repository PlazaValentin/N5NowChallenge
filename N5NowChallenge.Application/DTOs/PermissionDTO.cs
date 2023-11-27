using N5NowChallenge.Application.Commands.Base;

namespace N5NowChallenge.Application.DTOs;

public record PermissionDTO
{
    public int Id { get; set; }
    public string FirstNameEmployee { get; set; } = string.Empty;
    public string LastNameEmployee { get; set; } = string.Empty;
    public int PermissionTypeId { get; set; }
    public DateTime DatePermission { get; set; }
}