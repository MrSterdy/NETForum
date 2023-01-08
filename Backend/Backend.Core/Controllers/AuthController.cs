using System.Security.Claims;

using Backend.Core.Models.Auth;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser<int>> _manager;

    public AuthController(UserManager<IdentityUser<int>> manager) =>
        _manager = manager;
    
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromForm] AuthUser user)
    {
        var found = await _manager.FindByNameAsync(user.UserName);

        if (
            found is null || 
            _manager.PasswordHasher.VerifyHashedPassword(
                found, 
                found.PasswordHash!, 
                user.Password
            ) != PasswordVerificationResult.Success
        ) 
            return BadRequest();

        var claimsIdentity = new ClaimsIdentity(
            new[] { new Claim("ID", found.Id.ToString()) }, 
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity)
        );

        return Ok();
    }
    
    [HttpPost("Logout")]
    public async void Logout() =>
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] RegisterUser user)
    {
        var result = await _manager.CreateAsync(
            new IdentityUser<int> { UserName = user.UserName, Email = user.Email },
            user.Password
        );

        if (!result.Succeeded)
            return BadRequest();

        return Ok();
    }
}