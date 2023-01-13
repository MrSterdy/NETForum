using Backend.Core.Database.Repositories;
using Backend.Core.Identity;
using Backend.Core.Models;
using Backend.Core.Models.Thread;
using Backend.Core.Models.User;
using Thread = Backend.Core.Database.Entities.Thread;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class ThreadController : ControllerBase
{
    private readonly IThreadRepository _repository;

    private readonly UserManager<ApplicationUser> _userManager;

    public ThreadController(IThreadRepository repository, UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    [HttpGet("Id/{id:int}")]
    public async Task<ActionResult<ThreadResponse>> GetByIdAsync(int id) 
    {
        var thread = await _repository.GetByIdAsync(id);
        
        if (thread is null)
            return NotFound();

        var user = thread.User;

        return new ThreadResponse(
            id,
            new UserResponse(user.Id, user.Email!, user.UserName!),
            thread.Title,
            thread.Content
        );
    }
    
    [HttpGet("Page/{page:int}")]
    public async Task<Page<ThreadResponse>> GetByPageAsync(int page)
    {
        var res = await _repository.GetByPageAsync(page);

        return new Page<ThreadResponse>(
            res.Items.Select(t => new ThreadResponse(
                t.Id,
                new UserResponse(t.UserId, t.User.Email!, t.User.UserName!),
                t.Title,
                t.Content
            )),
            res.IsLast
        );
    }

    [HttpGet("User/{userId:int}")]
    public async Task<Page<ThreadResponse>> GetByUserIdAsync(int userId, [FromQuery] int page)
    {
        var res = await _repository.GetByUserIdAsync(userId, page);
        
        return new Page<ThreadResponse>(
            res.Items.Select(t => new ThreadResponse(
                t.Id,
                new UserResponse(t.UserId, t.User.Email!, t.User.UserName!),
                t.Title,
                t.Content
            )),
            res.IsLast
        );
    }

    [HttpPost]
    [Authorize]
    public async Task Create([FromBody] ThreadRequest model) =>
        await _repository.AddAsync(new Thread(
            int.Parse(_userManager.GetUserId(User)!),
            model.Title,
            model.Content
        ));
}