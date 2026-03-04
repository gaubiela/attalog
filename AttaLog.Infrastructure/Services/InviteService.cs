using System.Security.Cryptography;
using AttaLog.Application.DTOs.Invite;
using AttaLog.Application.Interfaces;
using AttaLog.Domain.Entities;
using AttaLog.Domain.Enums;
using AttaLog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AttaLog.Infrastructure.Services;

public class InviteService : IInviteService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public InviteService(AppDbContext context, IConfiguration configuration, IEmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<InviteResponse> SendInviteAsync(Guid senderUserId, SendInviteRequest request)
    {
        var membership = await _context.GroupMembers
            .Include(gm => gm.Group)
            .FirstOrDefaultAsync(gm => gm.UserId == senderUserId && !gm.Group.IsDeleted)
            ?? throw new InvalidOperationException("You are not in a group.");

        var pendingExists = await _context.GroupInvites
            .AnyAsync(gi => gi.GroupId == membership.GroupId
                && gi.InvitedEmail == request.Email
                && gi.Status == InviteStatus.Pending);

        if (pendingExists)
            throw new InvalidOperationException("A pending invite already exists for this email.");

        var expiryDays = int.Parse(_configuration["App:InviteExpiryDays"] ?? "7");
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));

        var invite = new GroupInvite
        {
            Id = Guid.NewGuid(),
            GroupId = membership.GroupId,
            InvitedById = senderUserId,
            InvitedEmail = request.Email,
            Token = token,
            Status = InviteStatus.Pending,
            ExpiresAt = DateTime.UtcNow.AddDays(expiryDays),
            CreatedAt = DateTime.UtcNow
        };

        _context.GroupInvites.Add(invite);
        await _context.SaveChangesAsync();

        var frontendOrigin = _configuration["App:FrontendOrigin"] ?? "http://localhost:4200";
        var inviteUrl = $"{frontendOrigin}/invite/{token}";
        var groupName = membership.Group.Name ?? "Unnamed group";

        await _emailService.SendInviteEmailAsync(request.Email, request.Email, groupName, inviteUrl);

        return MapToInviteResponse(invite);
    }

    public async Task<List<InviteResponse>> GetPendingInvitesAsync(Guid userId)
    {
        var membership = await _context.GroupMembers
            .FirstOrDefaultAsync(gm => gm.UserId == userId && !gm.Group.IsDeleted)
            ?? throw new InvalidOperationException("You are not in a group.");

        var invites = await _context.GroupInvites
            .Where(gi => gi.GroupId == membership.GroupId && gi.Status == InviteStatus.Pending)
            .OrderByDescending(gi => gi.CreatedAt)
            .ToListAsync();

        return invites.Select(MapToInviteResponse).ToList();
    }

    public async Task CancelInviteAsync(Guid userId, Guid inviteId)
    {
        var membership = await _context.GroupMembers
            .FirstOrDefaultAsync(gm => gm.UserId == userId && !gm.Group.IsDeleted)
            ?? throw new InvalidOperationException("You are not in a group.");

        var invite = await _context.GroupInvites
            .FirstOrDefaultAsync(gi => gi.Id == inviteId
                && gi.GroupId == membership.GroupId
                && gi.Status == InviteStatus.Pending)
            ?? throw new InvalidOperationException("Invite not found.");

        invite.Status = InviteStatus.Expired;
        invite.RespondedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<InvitePreviewResponse> GetInvitePreviewAsync(string token)
    {
        var invite = await _context.GroupInvites
            .Include(gi => gi.Group)
            .Include(gi => gi.InvitedBy)
            .FirstOrDefaultAsync(gi => gi.Token == token
                && gi.Status == InviteStatus.Pending
                && gi.ExpiresAt > DateTime.UtcNow)
            ?? throw new InvalidOperationException("Invite not found or expired.");

        return new InvitePreviewResponse
        {
            GroupName = invite.Group.Name,
            InvitedByName = invite.InvitedBy.Name,
            ExpiresAt = invite.ExpiresAt
        };
    }

    public async Task AcceptInviteAsync(Guid userId, string token)
    {
        var invite = await _context.GroupInvites
            .FirstOrDefaultAsync(gi => gi.Token == token
                && gi.Status == InviteStatus.Pending
                && gi.ExpiresAt > DateTime.UtcNow)
            ?? throw new InvalidOperationException("Invite not found or expired.");

        var alreadyInGroup = await _context.GroupMembers
            .AnyAsync(gm => gm.UserId == userId && !gm.Group.IsDeleted);

        if (alreadyInGroup)
            throw new InvalidOperationException("You must leave your current group before accepting this invite.");

        var member = new GroupMember
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            GroupId = invite.GroupId,
            Role = MemberRole.Member,
            JoinedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.GroupMembers.Add(member);
        invite.Status = InviteStatus.Accepted;
        invite.RespondedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task DeclineInviteAsync(Guid userId, string token)
    {
        var invite = await _context.GroupInvites
            .FirstOrDefaultAsync(gi => gi.Token == token
                && gi.Status == InviteStatus.Pending
                && gi.ExpiresAt > DateTime.UtcNow)
            ?? throw new InvalidOperationException("Invite not found or expired.");

        invite.Status = InviteStatus.Declined;
        invite.RespondedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task ExpireInvitesAsync()
    {
        var expired = await _context.GroupInvites
            .Where(gi => gi.Status == InviteStatus.Pending && gi.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        foreach (var invite in expired)
            invite.Status = InviteStatus.Expired;

        await _context.SaveChangesAsync();
    }

    private static InviteResponse MapToInviteResponse(GroupInvite invite)
    {
        return new InviteResponse
        {
            Id = invite.Id,
            InvitedEmail = invite.InvitedEmail,
            Status = invite.Status,
            CreatedAt = invite.CreatedAt,
            ExpiresAt = invite.ExpiresAt
        };
    }
}
