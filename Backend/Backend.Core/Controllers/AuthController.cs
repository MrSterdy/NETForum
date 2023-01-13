using Backend.Core.Mail;
using Backend.Core.Models.User;
using Backend.Core.Models.User.Auth;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser<int>> _userManager;
    private readonly SignInManager<IdentityUser<int>> _signInManager;

    private readonly IMailService _mailService;

    public AuthController(
        UserManager<IdentityUser<int>> userManager, 
        SignInManager<IdentityUser<int>> signInManager, 
        IMailService mailService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        
        _mailService = mailService;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<UserResponse>> Login([FromBody] LoginUserRequest user)
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
            return NotFound();

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            user.Password,
            user.RememberMe,
            false
        );

        if (!result.Succeeded)
            return BadRequest();

        var iUser = await _userManager.FindByNameAsync(user.UserName);

        return new UserResponse(iUser!.Id, iUser.Email!, iUser.UserName!, iUser.EmailConfirmed);
    }
    
    [HttpPost("Logout")]
    public async void Logout() =>
        await _signInManager.SignOutAsync();

    [AllowAnonymous]
    [HttpPost("Signup")]
    public async Task<ActionResult<UserResponse>> Signup([FromBody] SignupUserRequest user)
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
            return NotFound();
        
        if (
            await _userManager.FindByNameAsync(user.UserName) is not null ||
            await _userManager.FindByEmailAsync(user.Email) is not null
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

            await _mailService.SendMailAsync(iUser.Email, "Confirm email", url!);
        }
        else 
            return BadRequest(result.Errors);

        return new UserResponse(iUser.Id, iUser.Email, iUser.UserName, iUser.EmailConfirmed);
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