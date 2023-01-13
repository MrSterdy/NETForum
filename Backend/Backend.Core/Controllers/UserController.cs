using Backend.Core.Identity;
using Backend.Core.Models.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _manager;

    public UserController(UserManager<ApplicationUser> manager) =>
        _manager = manager;

    [HttpGet("Id/{id:int}")]
    public async Task<ActionResult<UserResponse>> GetByIdAsync(int id)
    {
        var iUser = await _manager.FindByIdAsync(id.ToString());

        if (iUser is null)
            return NotFound();

        return new UserResponse(iUser.Id, iUser.Email!, iUser.UserName!);
    }
}