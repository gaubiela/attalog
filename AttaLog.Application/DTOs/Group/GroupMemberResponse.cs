using AttaLog.Domain.Enums;

namespace AttaLog.Application.DTOs.Group;

public class GroupMemberResponse
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public MemberRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
}
