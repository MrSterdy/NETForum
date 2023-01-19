using System.ComponentModel.DataAnnotations;

using Backend.Core.Identity;
using Backend.Core.Mail;
using Backend.Core.Models.User.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Backend.Core.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IMailService _mailService;

    public AccountController(UserManager<ApplicationUser> userManager, IMailService mailService)
    {
        _userManager = userManager;
        _mailService = mailService;
    }

    [HttpGet]
    public async Task<AccountResponse> GetAsync()
    {
        var found = await _userManager.GetUserAsync(User);

        return new AccountResponse(found!.Id, found.Email!, found.EmailConfirmed, found.UserName!);
    }
    
    [HttpPatch("Email")]
    public async Task<IActionResult> ConfirmChangeEmail(string code, [FromBody] ChangeEmailRequest model)
    {
        var user = await _userManager.GetUserAsync(User);

        var result = await _userManager.ChangeEmailAsync(user!, model.Email, code);

        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }

    [HttpPost("Change/Email")]
    public async Task<IActionResult> ChangeEmail([Url] string clientUrl, [FromBody] ChangeEmailRequest model)
    {
        var user = await _userManager.GetUserAsync(User);

        var url = QueryHelpers.AddQueryString(clientUrl, new Dictionary<string, string?>
        {
            {"code", await _userManager.GenerateChangeEmailTokenAsync(user!, model.Email)},
            {"newEmail", model.Email}
        });

        await _mailService.SendMailAsync(user!.Email!, "Change email", url);

        return Ok();
    }

    [HttpPatch("UserName")]
    public async Task ChangeUserName([FromBody] ChangeUserNameRequest model)
    {
        var user = await _userManager.GetUserAsync(User);
        user!.UserName = model.UserName;

        await _userManager.UpdateAsync(user);
    }
    
    [HttpPatch("Password")]
    public async Task<IActionResult> ConfirmChangePassword(string code, [FromBody] ChangePasswordRequest model)
    {
        var user = await _userManager.GetUserAsync(User);

        var result = await _userManager.ResetPasswordAsync(user!, code, model.Password);

        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }

    [HttpPost("Change/Password")]
    public async Task<IActionResult> ChangePassword([Url] string clientUrl)
    {
        var user = await _userManager.GetUserAsync(User);

        var url = QueryHelpers.AddQueryString(clientUrl, new Dictionary<string, string?>
        {
            {"code", await _userManager.GeneratePasswordResetTokenAsync(user!)}
        });

        await _mailService.SendMailAsync(user!.Email!, "Reset password", url);

        return Ok();
    }
}