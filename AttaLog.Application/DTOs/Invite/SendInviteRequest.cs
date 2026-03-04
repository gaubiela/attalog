using System.ComponentModel.DataAnnotations;

namespace AttaLog.Application.DTOs.Invite;

public class SendInviteRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
