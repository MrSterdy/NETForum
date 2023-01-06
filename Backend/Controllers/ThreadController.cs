using Backend.Database.Entities;
using Backend.Database.Repositories;
using Thread = Backend.Database.Entities.Thread;

using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ThreadController : ControllerBase
{
    private readonly IThreadRepository _repository;

    public ThreadController(IThreadRepository repository) =>
        _repository = repository;

    [HttpGet("id/{id:int}")]
    public async Task<Thread?> GetByIdAsync(int id) =>
        await _repository.GetByIdAsync(id);

    [HttpGet("page/{page:int}")]
    public async Task<Page<Thread>> GetByPageAsync(int page) => 
        await _repository.GetByPageAsync(page);

    [HttpGet("user/{userId:int}")]
    public async Task<Page<Thread>> GetByUserIdAsync(int userId, [FromQuery] int page) =>
        await _repository.GetByUserIdAsync(userId, page);
}