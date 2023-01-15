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

    public async Task<Page<CommentResponse>> GetByPage(int page, int thread)
    {
        var rawPage = await _commentRepository.GetByPageAsync(page, thread);

        return new Page<CommentResponse>(
            rawPage.Items.Select(c => new CommentResponse(
                c.Id,
                new UserResponse(c.UserId, c.User.Email!, c.User.UserName!),
                c.ThreadId,
                c.Content
            )),
            rawPage.IsLast
        );
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CommentRequest model)
    {
        if (!await _threadRepository.Exists(model.ThreadId))
            return NotFound();

        await _commentRepository.AddAsync(new Comment
        {
            UserId = int.Parse(_userManager.GetUserId(User)!),
            ThreadId = model.ThreadId,
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

        if (comment.UserId != int.Parse(_userManager.GetUserId(User)!))
            return Forbid();

        await _commentRepository.DeleteAsync(comment);

        return Ok();
    }
}