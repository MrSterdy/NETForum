using System.Security.Claims;

using Backend.Core.Mail;
using Backend.Core.Models;
using Backend.Core.Models.Auth;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser<int>> _userManager;

    private readonly IMailService _mailService;

    public AuthController(UserManager<IdentityUser<int>> userManager, IMailService mailService)
    {
        _userManager = userManager;
        _mailService = mailService;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] AuthUser user)
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
            return NotFound();
        
        var found = await _userManager.FindByNameAsync(user.UserName);
        if (
            found is null || 
            !found.EmailConfirmed ||
            _userManager.PasswordHasher.VerifyHashedPassword(
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
    public async Task<ActionResult<User>> Register([FromBody] RegisterUser user)
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
            return NotFound();
        
        if (
            await _userManager.FindByNameAsync(user.UserName) is not null ||
            await _userManager.FindByEmailAsync(user.Email!) is not null
        )
            return Conflict();
        
        var iUser = new IdentityUser<int> { UserName = user.UserName, Email = user.Email };
        
        var result = await _userManager.CreateAsync(iUser, user.Password);

        if (result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(iUser);
            var url = Url.Action(
                "Confirm",
                "Auth",
                new { userId = iUser.Id, code },
                protocol: HttpContext.Request.Scheme
            );

            await _mailService.SendMailAsync(iUser.Email!, "Email Verification", url!);
        }
        else 
            return BadRequest(result.Errors);

        return new User
        {
            Id = iUser.Id,
            Email = iUser.Email,
            UserName = iUser.UserName
        };
    }
    
    [AllowAnonymous]
    [HttpGet("Confirm")]
    public async Task<IActionResult> ConfirmAsync(int userId, string code)
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
            return NotFound();
        
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NotFound();

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok();
    }
}