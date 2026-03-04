using AttaLog.Application.DTOs.Group;

namespace AttaLog.Application.Interfaces;

public interface IGroupService
{
    Task<GroupResponse?> GetGroupAsync(Guid userId);
    Task<GroupResponse> CreateGroupAsync(Guid userId, CreateGroupRequest request);
    Task<GroupResponse> UpdateGroupAsync(Guid userId, UpdateGroupRequest request);
    Task LeaveGroupAsync(Guid userId);
    Task RemoveMemberAsync(Guid adminUserId, Guid targetUserId);
    Task<List<GroupMemberResponse>> GetMembersAsync(Guid userId);
}
