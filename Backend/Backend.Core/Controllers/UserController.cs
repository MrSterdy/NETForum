using Backend.Core.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser<int>> _manager;

    public UserController(UserManager<IdentityUser<int>> manager) =>
        _manager = manager;

    [HttpGet("Id/{id:int}")]
    public async Task<ActionResult<User>> GetByIdAsync(int id)
    {
        var iUser = await _manager.FindByIdAsync(id.ToString());

        if (iUser is null)
            return NotFound();

        return new User
        {
            Id = id,
            Email = iUser.Email!,
            EmailConfirmed = iUser.EmailConfirmed,
            UserName = iUser.UserName!
        };
    }
    
    [Authorize]
    [HttpGet("Current")]
    public async Task<User> GetCurrentAsync()
    {
        var found = await _manager.FindByIdAsync(HttpContext.User.Claims.First().Value);

        return new User
        {
            Id = found!.Id,
            Email = found.Email!,
            EmailConfirmed = found.EmailConfirmed,
            UserName = found.UserName!
        };
    }
}