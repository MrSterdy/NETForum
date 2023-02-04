using System.ComponentModel.DataAnnotations;

using AutoMapper;

using NETForum.Infrastructure.Identity;
using NETForum.Infrastructure.Mail;
using NETForum.Infrastructure.Database.Repositories;
using NETForum.Models.Requests;
using NETForum.Models.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace NETForum.Core.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserRepository _repository;
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly IMailService _mailService;

    private readonly IMapper _mapper;

    public AccountController(
        IUserRepository repository,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMailService mailService,
        IMapper mapper
    )
    {
        _repository = repository;
        
        _userManager = userManager;
        _signInManager = signInManager;
        
        _mailService = mailService;

        _mapper = mapper;
    }

    [HttpGet]
    public async Task<AccountResponse> Get() =>
        _mapper.Map<AccountResponse>(await _userManager.GetUserAsync(User));

    [AllowAnonymous]
    [HttpPost("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(int userId, string code)
    {
        if (User.Identity!.IsAuthenticated)
            return Forbid();
        
        var user = await _repository.GetByIdAsync(userId);
        if (user is null)
            return NotFound();

        var result = await _userManager.ConfirmEmailAsync(user, code);

        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }
    
    [HttpPut("Email")]
    public async Task<IActionResult> ChangeEmail(string code)
    {
        var user = await _userManager.GetUserAsync(User);

        var result = await _userManager.ChangeEmailAsync(user!, user!.NewEmail!, code);
        if (!result.Succeeded) 
            return BadRequest(result.Errors);
        
        user.NewEmail = null;

        await _repository.UpdateAsync(user);

        return Ok();
    }

    [HttpPost("ChangeEmail")]
    public async Task<IActionResult> RequestChangeEmail([Url] string callbackUrl, [FromBody] RequiredEmailRequest model)
    {
        var user = await _userManager.GetUserAsync(User);
        user!.NewEmail = model.Email;

        await _repository.UpdateAsync(user);

        var url = QueryHelpers.AddQueryString(callbackUrl, new Dictionary<string, string?>
        {
            {"code", await _userManager.GenerateChangeEmailTokenAsync(user, model.Email)}
        });

        await _mailService.SendMailAsync(user.Email!, "Change email", url);

        return Ok();
    }

    [HttpPut("UserName")]
    public async Task ChangeUserName([FromBody] ChangeUserNameRequest model)
    {
        var user = await _userManager.GetUserAsync(User);
        user!.UserName = model.UserName;

        await _repository.UpdateAsync(user);
    }
    
    [AllowAnonymous]
    [HttpPut("Password")]
    public async Task<IActionResult> ChangePassword(int? userId, string? code, [FromBody] ChangePasswordRequest model)
    {
        ApplicationUser? user;
        if (User.Identity!.IsAuthenticated)
            user = await _userManager.GetUserAsync(User);
        else if (userId is null || code is null)
            return BadRequest();
        else 
            user = await _userManager.FindByIdAsync(userId.ToString()!);

        if (user is null)
            return NotFound();

        IdentityResult result;
        if (code is not null)
            result = await _userManager.ResetPasswordAsync(user, code, model.NewPassword);
        else if (model.Password is not null)
            result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
        else
            return BadRequest();

        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }

    [AllowAnonymous]
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> RequestResetPassword([Url] string callbackUrl, [FromBody] RequiredEmailRequest model)
    {
        var user = User.Identity!.IsAuthenticated
            ? await _userManager.GetUserAsync(User)
            : await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
            return NotFound();
        
        var url = QueryHelpers.AddQueryString(callbackUrl, new Dictionary<string, string?>
        {
            {"userId", user.Id.ToString()},
            {"code", await _userManager.GeneratePasswordResetTokenAsync(user)},
        });

        await _mailService.SendMailAsync(user.Email!, "Reset password", url);

        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost("Signup")]
    public async Task<ActionResult<AccountResponse>> Signup([Url] string callbackUrl, [FromBody] SignupRequest user)
    {
        if (User.Identity!.IsAuthenticated)
            return Forbid();
        
        if (
            await _userManager.FindByNameAsync(user.UserName) is not null ||
            await _userManager.FindByEmailAsync(user.Email) is not null
        )
            return Conflict();
        
        var iUser = new ApplicationUser { UserName = user.UserName, Email = user.Email };
        
        var result = await _userManager.CreateAsync(iUser, user.Password);
        if (!result.Succeeded) 
            return BadRequest(result.Errors);

        var url = QueryHelpers.AddQueryString(callbackUrl, new Dictionary<string, string?>
        {
            {"userId", iUser.Id.ToString()},
            {"code", await _userManager.GenerateEmailConfirmationTokenAsync(iUser)}
        });

        await _mailService.SendMailAsync(iUser.Email, "Confirm email", url);

        return _mapper.Map<AccountResponse>(iUser);
    }
    
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<AccountResponse>> Login([FromBody] LoginRequest user)
    {
        if (User.Identity!.IsAuthenticated)
            return Forbid();

        var iUser = await _userManager.FindByNameAsync(user.UserName);

        if (iUser is null || iUser.Banned)
            return BadRequest();

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            user.Password,
            user.RememberMe,
            lockoutOnFailure: true
        );
        
        if (result.IsLockedOut)
            return new StatusCodeResult(429);
        
        if (!result.Succeeded)
            return BadRequest();

        return _mapper.Map<AccountResponse>(await _userManager.FindByNameAsync(user.UserName));
    }
    
    [HttpPost("Logout")]
    public async Task Logout() => await _signInManager.SignOutAsync();
}