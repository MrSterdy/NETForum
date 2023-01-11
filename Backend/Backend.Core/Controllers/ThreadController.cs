using Backend.Core.Database.Repositories;
using Backend.Core.Models;
using Backend.Core.Models.Thread;
using Backend.Core.Models.User;

using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class ThreadController : ControllerBase
{
    private readonly IThreadRepository _repository;

    public ThreadController(IThreadRepository repository) =>
        _repository = repository;

    [HttpGet("Id/{id:int}")]
    public async Task<ActionResult<ThreadResponse>> GetByIdAsync(int id) 
    {
        var thread = await _repository.GetByIdAsync(id);
        
        if (thread is null)
            return NotFound();

        var user = thread.User;

        return new ThreadResponse(
            id,
            new UserResponse(user.Id, user.Email!, user.UserName!, user.EmailConfirmed),
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
                new UserResponse(t.UserId, t.User.Email!, t.User.UserName!, t.User.EmailConfirmed),
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
                new UserResponse(t.UserId, t.User.Email!, t.User.UserName!, t.User.EmailConfirmed),
                t.Title,
                t.Content
            )),
            res.IsLast
        );
    }
}