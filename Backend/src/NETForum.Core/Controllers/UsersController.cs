using AutoMapper;

using NETForum.Models.Responses;
using NETForum.Infrastructure.Database.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NETForum.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repository;

    private readonly IMapper _mapper;

    public UsersController(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetById(int id)
    {
        var iUser = await _repository.GetByIdAsync(id);

        return iUser is null ? NotFound() : _mapper.Map<UserResponse>(iUser);
    }

    [HttpPost("Ban/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BanById(int id)
    {
        var iUser = await _repository.GetByIdAsync(id);

        if (iUser is null)
            return NotFound();

        iUser.Banned = !iUser.Banned;

        await _repository.UpdateAsync(iUser);

        return Ok();
    }
}