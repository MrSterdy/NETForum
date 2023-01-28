using AutoMapper;

using Backend.Core.Identity;
using Backend.Core.Models.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _manager;

    private readonly IMapper _mapper;

    public UsersController(UserManager<ApplicationUser> manager, IMapper mapper)
    {
        _manager = manager;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetById(int id)
    {
        var iUser = await _manager.FindByIdAsync(id.ToString());

        return iUser is null ? NotFound() : _mapper.Map<UserResponse>(iUser);
    }

    [HttpPost("Ban/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BanById(int id)
    {
        var iUser = await _manager.FindByIdAsync(id.ToString());

        if (iUser is null)
            return NotFound();

        iUser.Banned = !iUser.Banned;

        await _manager.UpdateAsync(iUser);

        return Ok();
    }
}