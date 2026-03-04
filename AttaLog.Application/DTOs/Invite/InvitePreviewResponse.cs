namespace AttaLog.Application.DTOs.Invite;

public class InvitePreviewResponse
{
    public string? GroupName { get; set; }
    public string InvitedByName { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
