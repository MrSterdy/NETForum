using Backend.Core.Database.Entities;
using Backend.Core.Database.Repositories;
using Backend.Core.Identity;
using Backend.Core.Models;
using Backend.Core.Models.Comment;
using Backend.Core.Models.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IThreadRepository _threadRepository;
    private readonly ICommentRepository _commentRepository;

    private readonly UserManager<ApplicationUser> _userManager;

    public CommentsController(
        ICommentRepository commentRepository, 
        IThreadRepository threadRepository, 
        UserManager<ApplicationUser> userManager
    )
    {
        _commentRepository = commentRepository;
        _threadRepository = threadRepository;
        
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<Page<CommentResponse>> GetByPage(int page, int threadId)
    {
        var rawPage = await _commentRepository.GetByPageAsync(page, threadId);

        var comments = new List<CommentResponse>();
        foreach (var comment in rawPage.Items)
            comments.Add(new CommentResponse(
                comment.Id,
                comment.CreatedDate,
                new UserResponse(
                    comment.UserId,
                    comment.User.UserName!,
                    comment.User.Enabled,
                    await _userManager.IsInRoleAsync(comment.User, "Admin")
                ),
                comment.ThreadId,
                comment.Content
            ));

        return new Page<CommentResponse>(comments, rawPage.IsLast);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(int threadId, [FromBody] CommentRequest model)
    {
        if (!await _threadRepository.Exists(threadId))
            return NotFound();

        await _commentRepository.AddAsync(new Comment
        {
            UserId = int.Parse(_userManager.GetUserId(User)!),
            ThreadId = threadId,
            Content = model.Content
        });

        return Ok();
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateById(int id, [FromBody] CommentRequest model)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment is null)
            return NotFound();

        var user = int.Parse(_userManager.GetUserId(User)!);

        if (comment.UserId != user)
            return Forbid();

        await _commentRepository.UpdateAsync(new Comment
        {
            Id = id,
            CreatedDate = comment.CreatedDate,
            UserId = user,
            ThreadId = comment.ThreadId,
            Content = model.Content
        });

        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment is null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);

        if (comment.UserId == user!.Id || await _userManager.IsInRoleAsync(user, "Admin"))
        {
            await _commentRepository.DeleteAsync(comment);

            return Ok();
        }
        
        return Forbid();
    }
}