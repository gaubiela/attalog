using AttaLog.Application.DTOs.Group;
using AttaLog.Application.Interfaces;
using AttaLog.Domain.Entities;
using AttaLog.Domain.Enums;
using AttaLog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AttaLog.Infrastructure.Services;

public class GroupService : IGroupService
{
    private readonly AppDbContext _context;

    public GroupService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GroupResponse?> GetGroupAsync(Guid userId)
    {
        var membership = await _context.GroupMembers
            .Include(gm => gm.Group)
            .ThenInclude(g => g.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(gm => gm.UserId == userId && !gm.Group.IsDeleted);

        if (membership == null)
            return null;

        return MapToGroupResponse(membership.Group);
    }

    public async Task<GroupResponse> CreateGroupAsync(Guid userId, CreateGroupRequest request)
    {
        var existing = await _context.GroupMembers
            .AnyAsync(gm => gm.UserId == userId && !gm.Group.IsDeleted);

        if (existing)
            throw new InvalidOperationException("You are already in a group.");

        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedById = userId,
            CreatedAt = DateTime.UtcNow
        };

        var member = new GroupMember
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            GroupId = group.Id,
            Role = MemberRole.Admin,
            JoinedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.Groups.Add(group);
        _context.GroupMembers.Add(member);
        await _context.SaveChangesAsync();

        var saved = await _context.Groups
            .Include(g => g.Members)
            .ThenInclude(m => m.User)
            .FirstAsync(g => g.Id == group.Id);

        return MapToGroupResponse(saved);
    }

    public async Task<GroupResponse> UpdateGroupAsync(Guid userId, UpdateGroupRequest request)
    {
        var membership = await _context.GroupMembers
            .Include(gm => gm.Group)
            .ThenInclude(g => g.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(gm => gm.UserId == userId && !gm.Group.IsDeleted)
            ?? throw new InvalidOperationException("You are not in a group.");

        if (membership.Role != MemberRole.Admin)
            throw new UnauthorizedAccessException("Only admins can update the group.");

        membership.Group.Name = request.Name;
        await _context.SaveChangesAsync();

        return MapToGroupResponse(membership.Group);
    }

    public async Task LeaveGroupAsync(Guid userId)
    {
        var membership = await _context.GroupMembers
            .Include(gm => gm.Group)
            .ThenInclude(g => g.Members)
            .FirstOrDefaultAsync(gm => gm.UserId == userId && !gm.Group.IsDeleted)
            ?? throw new InvalidOperationException("You are not in a group.");

        var group = membership.Group;
        _context.GroupMembers.Remove(membership);

        var remaining = group.Members.Where(m => m.UserId != userId).ToList();

        if (remaining.Count == 0)
        {
            group.IsDeleted = true;
        }
        else if (membership.Role == MemberRole.Admin)
        {
            var next = remaining.OrderBy(m => m.JoinedAt).First();
            next.Role = MemberRole.Admin;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveMemberAsync(Guid adminUserId, Guid targetUserId)
    {
        if (adminUserId == targetUserId)
            throw new InvalidOperationException("You cannot remove yourself. Use leave instead.");

        var adminMembership = await _context.GroupMembers
            .FirstOrDefaultAsync(gm => gm.UserId == adminUserId && !gm.Group.IsDeleted)
            ?? throw new InvalidOperationException("You are not in a group.");

        if (adminMembership.Role != MemberRole.Admin)
            throw new UnauthorizedAccessException("Only admins can remove members.");

        var targetMembership = await _context.GroupMembers
            .FirstOrDefaultAsync(gm => gm.UserId == targetUserId && gm.GroupId == adminMembership.GroupId)
            ?? throw new InvalidOperationException("User is not a member of your group.");

        _context.GroupMembers.Remove(targetMembership);
        await _context.SaveChangesAsync();
    }

    public async Task<List<GroupMemberResponse>> GetMembersAsync(Guid userId)
    {
        var membership = await _context.GroupMembers
            .FirstOrDefaultAsync(gm => gm.UserId == userId && !gm.Group.IsDeleted)
            ?? throw new InvalidOperationException("You are not in a group.");

        var members = await _context.GroupMembers
            .Include(gm => gm.User)
            .Where(gm => gm.GroupId == membership.GroupId)
            .OrderBy(gm => gm.JoinedAt)
            .ToListAsync();

        return members.Select(MapToMemberResponse).ToList();
    }

    private static GroupResponse MapToGroupResponse(Group group)
    {
        return new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            CreatedAt = group.CreatedAt,
            Members = group.Members.Select(MapToMemberResponse).ToList()
        };
    }

    private static GroupMemberResponse MapToMemberResponse(GroupMember m)
    {
        return new GroupMemberResponse
        {
            UserId = m.UserId,
            Name = m.User.Name,
            AvatarUrl = m.User.AvatarUrl,
            Role = m.Role,
            JoinedAt = m.JoinedAt
        };
    }
}
