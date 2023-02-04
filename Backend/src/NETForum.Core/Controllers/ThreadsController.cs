using AutoMapper;

using NETForum.Infrastructure.Identity;
using NETForum.Models.Requests;
using NETForum.Models.Responses;
using NETForum.Infrastructure.Database.Repositories;
using NETForum.Infrastructure.Database.Entities;
using Thread = NETForum.Infrastructure.Database.Entities.Thread;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace NETForum.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class ThreadsController : ControllerBase
{
    private readonly IThreadRepository _repository;

    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IMapper _mapper;

    public ThreadsController(
        IThreadRepository repository, 
        UserManager<ApplicationUser> userManager,
        IMapper mapper
    )
    {
        _repository = repository;
        
        _userManager = userManager;

        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ThreadResponse>> GetById(int id) 
    {
        var thread = await _repository.GetByIdAsync(id);

        if (thread is null)
            return NotFound();
        
        thread.Views++;
        await _repository.UpdateAsync(thread);

        return _mapper.Map<ThreadResponse>(thread);
    }

    [HttpGet]
    public async Task<ActionResult<Page<ThreadResponse>>> Search(
        int page,
        [FromQuery] int[] tagIds,
        int? userId,
        string? title
    ) => 
        _mapper.Map<Page<ThreadResponse>>(await _repository.SearchAsync(page, tagIds, userId, title));

    [HttpPost]
    [Authorize]
    public async Task Create([FromBody] ThreadRequest model)
    {
        var entity = new Thread
        {
            UserId = int.Parse(_userManager.GetUserId(User)!),
            Title = model.Title,
            Content = model.Content
        };

        await _repository.AddAsync(entity);

        if (model.TagIds.Length != 0)
        {
            entity.Tags = model.TagIds
                .Select(tagId => new ThreadTags { TagId = tagId, ThreadId = entity.Id })
                .ToList();

            await _repository.UpdateAsync(entity);
        }
    }

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

        thread.ModifiedDate = DateTimeOffset.UtcNow;
        thread.Title = model.Title;
        thread.Content = model.Content;
        thread.Tags = model.TagIds
            .Select(tagId => new ThreadTags { TagId = tagId, ThreadId = id })
            .ToList();

        await _repository.UpdateAsync(thread);

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
}