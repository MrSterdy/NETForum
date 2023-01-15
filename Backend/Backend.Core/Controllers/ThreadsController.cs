﻿using Backend.Core.Database.Repositories;
using Backend.Core.Identity;
using Backend.Core.Models;
using Backend.Core.Models.Comment;
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
            thread.Content,
            thread.Comments.Select(c => new CommentResponse(
                c.Id, 
                new UserResponse(c.UserId, c.User.Email!, c.User.UserName!), 
                c.ThreadId, 
                c.Content
            ))
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

        if (thread.UserId != int.Parse(_userManager.GetUserId(User)!))
            return Forbid();

        await _repository.DeleteAsync(thread);

        return Ok();
    }
    
    [HttpGet]
    public async Task<Page<ThreadResponse>> GetByPageAsync(int page, int? user)
    {
        var rawPage = user is null
            ? await _repository.GetByPageAsync(page)
            : await _repository.GetByUserIdAsync(user.Value, page);

        return new Page<ThreadResponse>(
            rawPage.Items.Select(t => new ThreadResponse(
                t.Id,
                new UserResponse(t.UserId, t.User.Email!, t.User.UserName!),
                t.Title,
                t.Content,
                t.Comments.Select(c => new CommentResponse(
                    c.Id, 
                    new UserResponse(c.UserId, c.User.Email!, c.User.UserName!), 
                    c.ThreadId, 
                    c.Content
                ))
            )),
            rawPage.IsLast
        );
    }
}