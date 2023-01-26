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
public class ThreadsController : ControllerBase
{
    private readonly IThreadRepository _repository;

    private readonly UserManager<ApplicationUser> _userManager;

    public ThreadsController(IThreadRepository repository, UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ThreadResponse>> GetById(int id) 
    {
        var thread = await _repository.GetByIdAsync(id);
        
        if (thread is null)
            return NotFound();

        var user = thread.User;

        return new ThreadResponse(
            id,
            thread.CreatedDate,
            new UserResponse(
                user.Id, 
                user.UserName!, 
                user.Enabled,
                await _userManager.IsInRoleAsync(user, "Admin")
            ),
            thread.Title,
            thread.Content
        );
    }
    
    [HttpPost]
    [Authorize]
    public async Task Create([FromBody] ThreadRequest model) =>
        await _repository.AddAsync(new Thread
        {
            UserId = int.Parse(_userManager.GetUserId(User)!),
            Title = model.Title,
            Content = model.Content
        });

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateById(int id, [FromBody] ThreadRequest model)
    {
        var thread = await _repository.GetByIdAsync(id);

        if (thread is null)
            return NotFound();

        var user = int.Parse(_userManager.GetUserId(User)!);

        if (thread.UserId != user)
            return Forbid();

        await _repository.UpdateAsync(new Thread
        {
            Id = id,
            CreatedDate = thread.CreatedDate,
            UserId = user,
            Title = model.Title,
            Content = model.Content
        });

        return Ok();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var thread = await _repository.GetByIdAsync(id);
        
        if (thread is null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);

        if (thread.UserId == user!.Id || await _userManager.IsInRoleAsync(user, "Admin"))
        {
            await _repository.DeleteAsync(thread);

            return Ok();
        }

        return Forbid();
    }
    
    [HttpGet]
    public async Task<Page<ThreadResponse>> GetByPage(int page, int? userId)
    {
        var rawPage = userId is null
            ? await _repository.GetByPageAsync(page)
            : await _repository.GetByUserIdAsync(userId.Value, page);

        var threads = new List<ThreadResponse>();
        foreach (var thread in rawPage.Items)
            threads.Add(new ThreadResponse(
                thread.Id,
                thread.CreatedDate,
                new UserResponse(
                    thread.UserId,
                    thread.User.UserName!,
                    thread.User.Enabled,
                    await _userManager.IsInRoleAsync(thread.User, "Admin")
                ),
                thread.Title,
                thread.Content
            ));

        return new Page<ThreadResponse>(threads, rawPage.IsLast);
    }
}