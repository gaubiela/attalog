using AttaLog.Domain.Enums;

namespace AttaLog.Application.DTOs.Invite;

public class InviteResponse
{
    public Guid Id { get; set; }
    public string InvitedEmail { get; set; } = string.Empty;
    public InviteStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}
