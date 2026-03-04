using AttaLog.Application.DTOs.Auth;
using AttaLog.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttaLog.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        SetRefreshTokenCookie(response.RefreshToken);

        return Ok(new { response.AccessToken, response.ExpiresAt });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        SetRefreshTokenCookie(response.RefreshToken);

        return Ok(new { response.AccessToken, response.ExpiresAt });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized();

        var response = await _authService.RefreshTokenAsync(refreshToken);
        SetRefreshTokenCookie(response.RefreshToken);

        return Ok(new { response.AccessToken, response.ExpiresAt });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (!string.IsNullOrEmpty(refreshToken))
            await _authService.RevokeRefreshTokenAsync(refreshToken);

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            Path = "/api/auth",
            HttpOnly = true
        });

        return NoContent();
    }

    private void SetRefreshTokenCookie(string token)
    {
        Response.Cookies.Append("refreshToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api/auth",
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });
    }
}
