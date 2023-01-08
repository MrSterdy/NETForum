using Backend.Core.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser<int>> _manager;

    public UserController(UserManager<IdentityUser<int>> manager) =>
        _manager = manager;

    [HttpGet("id/{id:int}")]
    public async Task<ActionResult<User>> GetByIdAsync(int id)
    {
        var iUser = await _manager.FindByIdAsync(id.ToString());

        if (iUser is null)
            return NotFound();

        return new User
        {
            UserName = iUser.UserName!,
            Email = iUser.Email!
        };
    }
}