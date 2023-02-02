﻿using AutoMapper;

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
        int? userId,
        string? title,
        [FromQuery] int[]? tagIds
    ) => _mapper.Map<Page<ThreadResponse>>(await _repository.SearchAsync(page, userId, title, tagIds));

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
            ModifiedDate = DateTimeOffset.UtcNow,
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
}