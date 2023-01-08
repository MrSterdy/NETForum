using Backend.Core.Database;
using Backend.Core.Database.Repositories;
using Thread = Backend.Core.Database.Entities.Thread;

using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ThreadController : ControllerBase
{
    private readonly IThreadRepository _repository;

    public ThreadController(IThreadRepository repository) =>
        _repository = repository;

    [HttpGet("id/{id:int}")]
    public async Task<ActionResult<Thread>> GetByIdAsync(int id) 
    {
        var thread = await _repository.GetByIdAsync(id);
        if (thread is null)
            return NotFound();
        return thread;
    }
        

    [HttpGet("page/{page:int}")]
    public async Task<Page<Thread>> GetByPageAsync(int page) => 
        await _repository.GetByPageAsync(page);

    [HttpGet("user/{userId:int}")]
    public async Task<Page<Thread>> GetByUserIdAsync(int userId, [FromQuery] int page) =>
        await _repository.GetByUserIdAsync(userId, page);
}