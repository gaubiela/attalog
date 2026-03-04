using System.Security.Claims;
using AttaLog.Application.DTOs.Group;
using AttaLog.Application.DTOs.Invite;
using AttaLog.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttaLog.Api.Controllers;

[ApiController]
[Route("api/group")]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IInviteService _inviteService;

    public GroupController(IGroupService groupService, IInviteService inviteService)
    {
        _groupService = groupService;
        _inviteService = inviteService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetGroup()
    {
        var group = await _groupService.GetGroupAsync(GetUserId());
        return group == null ? NoContent() : Ok(group);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
    {
        var group = await _groupService.CreateGroupAsync(GetUserId(), request);
        return CreatedAtAction(nameof(GetGroup), group);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroupRequest request)
    {
        var group = await _groupService.UpdateGroupAsync(GetUserId(), request);
        return Ok(group);
    }

    [HttpDelete("leave")]
    public async Task<IActionResult> LeaveGroup()
    {
        await _groupService.LeaveGroupAsync(GetUserId());
        return NoContent();
    }

    [HttpGet("members")]
    public async Task<IActionResult> GetMembers()
    {
        var members = await _groupService.GetMembersAsync(GetUserId());
        return Ok(members);
    }

    [HttpDelete("members/{id:guid}")]
    public async Task<IActionResult> RemoveMember(Guid id)
    {
        await _groupService.RemoveMemberAsync(GetUserId(), id);
        return NoContent();
    }

    [HttpGet("invites")]
    public async Task<IActionResult> GetPendingInvites()
    {
        var invites = await _inviteService.GetPendingInvitesAsync(GetUserId());
        return Ok(invites);
    }

    [HttpPost("invites")]
    public async Task<IActionResult> SendInvite([FromBody] SendInviteRequest request)
    {
        var invite = await _inviteService.SendInviteAsync(GetUserId(), request);
        return CreatedAtAction(nameof(GetPendingInvites), invite);
    }

    [HttpDelete("invites/{id:guid}")]
    public async Task<IActionResult> CancelInvite(Guid id)
    {
        await _inviteService.CancelInviteAsync(GetUserId(), id);
        return NoContent();
    }
}
