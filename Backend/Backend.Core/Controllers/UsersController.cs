using Backend.Core.Identity;
using Backend.Core.Models.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _manager;

    public UsersController(UserManager<ApplicationUser> manager) =>
        _manager = manager;

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetById(int id)
    {
        var iUser = await _manager.FindByIdAsync(id.ToString());

        return iUser is null ? NotFound() : new UserResponse(
            iUser.Id, 
            iUser.UserName!,
            await _manager.IsInRoleAsync(iUser, "Admin")
        );
    }
}