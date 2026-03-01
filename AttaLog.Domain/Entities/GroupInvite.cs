using AttaLog.Domain.Enums;

namespace AttaLog.Domain.Entities;

public class GroupInvite : BaseEntity
{
    public Guid GroupId { get; set; }
    public Guid InvitedById { get; set; }
    public string InvitedEmail { get; set; }
    public string Token { get; set; }
    public InviteStatus Status { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RespondedAt { get; set; }

    public Group Group { get; set; }
    public User InvitedBy { get; set; }
}
