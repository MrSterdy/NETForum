using Backend.Core.Models.User;

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
    public async Task<ActionResult<UserResponse>> GetByIdAsync(int id)
    {
        var iUser = await _manager.FindByIdAsync(id.ToString());

        if (iUser is null)
            return NotFound();

        return new UserResponse(iUser.Id, iUser.Email!, iUser.UserName!, iUser.EmailConfirmed);
    }
    
    [Authorize]
    [HttpGet("Current")]
    public async Task<UserResponse> GetCurrentAsync()
    {
        var found = await _manager.FindByIdAsync(HttpContext.User.Claims.First().Value);

        return new UserResponse(found!.Id, found.Email!, found.UserName!, found.EmailConfirmed);
    }
}