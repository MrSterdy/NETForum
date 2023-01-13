﻿using Backend.Core.Identity;
using Backend.Core.Mail;
using Backend.Core.Models.User.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly IMailService _mailService;

    public AuthController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager, 
        IMailService mailService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        
        _mailService = mailService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest user)
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
            return NotFound();

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            user.Password,
            user.RememberMe,
            false
        );

        return result.Succeeded ? Ok() : BadRequest();
    }
    
    [Authorize]
    [HttpPost("Logout")]
    public async void Logout() =>
        await _signInManager.SignOutAsync();

    [HttpPost("Signup")]
    public async Task<IActionResult> Signup([FromBody] SignupUserRequest user)
    {
        if (HttpContext.User.Identity!.IsAuthenticated)
            return NotFound();
        
        if (
            await _userManager.FindByNameAsync(user.UserName) is not null ||
            await _userManager.FindByEmailAsync(user.Email) is not null
        )
            return Conflict();
        
        var iUser = new ApplicationUser { UserName = user.UserName, Email = user.Email };
        
        var result = await _userManager.CreateAsync(iUser, user.Password);

        if (!result.Succeeded) 
            return BadRequest(result.Errors);
        
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(iUser);
        var url = Url.Action(
            "Confirm",
            "Auth",
            new { userId = iUser.Id, code },
            protocol: HttpContext.Request.Scheme
        );

        await _mailService.SendMailAsync(iUser.Email, "Confirm email", url!);

        return Ok();
    }

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