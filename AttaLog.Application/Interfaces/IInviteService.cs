using AttaLog.Application.DTOs.Invite;

namespace AttaLog.Application.Interfaces;

public interface IInviteService
{
    Task<InviteResponse> SendInviteAsync(Guid senderUserId, SendInviteRequest request);
    Task<List<InviteResponse>> GetPendingInvitesAsync(Guid userId);
    Task CancelInviteAsync(Guid userId, Guid inviteId);
    Task<InvitePreviewResponse> GetInvitePreviewAsync(string token);
    Task AcceptInviteAsync(Guid userId, string token);
    Task DeclineInviteAsync(Guid userId, string token);
    Task ExpireInvitesAsync();
}
