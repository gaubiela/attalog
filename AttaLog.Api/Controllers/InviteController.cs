using System.Security.Claims;
using AttaLog.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttaLog.Api.Controllers;

[ApiController]
[Route("api/invites")]
public class InviteController : ControllerBase
{
    private readonly IInviteService _inviteService;

    public InviteController(IInviteService inviteService)
    {
        _inviteService = inviteService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("{token}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetInvitePreview(string token)
    {
        var preview = await _inviteService.GetInvitePreviewAsync(token);
        return Ok(preview);
    }

    [HttpPost("{token}/accept")]
    [Authorize]
    public async Task<IActionResult> AcceptInvite(string token)
    {
        await _inviteService.AcceptInviteAsync(GetUserId(), token);
        return NoContent();
    }

    [HttpPost("{token}/decline")]
    [Authorize]
    public async Task<IActionResult> DeclineInvite(string token)
    {
        await _inviteService.DeclineInviteAsync(GetUserId(), token);
        return NoContent();
    }
}
