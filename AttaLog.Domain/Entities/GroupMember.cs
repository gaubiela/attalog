using AttaLog.Domain.Enums;

namespace AttaLog.Domain.Entities;

public class GroupMember : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }
    public MemberRole Role { get; set; }
    public DateTime JoinedAt { get; set; }

    public User User { get; set; }
    public Group Group { get; set; }
}
