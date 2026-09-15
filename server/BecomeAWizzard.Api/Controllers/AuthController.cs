using System.Security.Claims;
using BecomeAWizzard.Api.DTOs;
using BecomeAWizzard.Api.Extensions;
using BecomeAWizzard.Api.Models;
using BecomeAWizzard.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BecomeAWizzard.Api.Controllers;

[ApiController, Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterRequest request)
    {
        try { var user = await authService.RegisterAsync(request); await SignIn(user); return Ok(ToDto(user)); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginRequest request)
    {
        var user = await authService.ValidateAsync(request);
        if (user is null) return Unauthorized(new { message = "Feil e-post eller passord." });
        await SignIn(user); return Ok(ToDto(user));
    }

    [Authorize, HttpGet("me")]
    public ActionResult<UserDto> Me() => Ok(new UserDto(User.GetUserId(), User.Identity?.Name ?? "Bruker", User.FindFirstValue(ClaimTypes.Email) ?? "", int.Parse(User.FindFirstValue("xp") ?? "0")));

    [Authorize, HttpPost("logout")]
    public async Task<IActionResult> Logout() { await HttpContext.SignOutAsync(); return NoContent(); }

    private async Task SignIn(User user)
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Name, user.DisplayName), new Claim(ClaimTypes.Email, user.Email), new Claim("xp", user.TotalXp.ToString()) };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
    }
    private static UserDto ToDto(User user) => new(user.Id, user.DisplayName, user.Email, user.TotalXp);
}

