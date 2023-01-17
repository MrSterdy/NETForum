using Backend.Core.Database.Entities;
using Backend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly Context _context;

    public CommentRepository(Context context) =>
        _context = context;

    public async Task<IEnumerable<Comment>> GetAllAsync() =>
        await _context.Comments
            .Include(c => c.User)
            .ToListAsync();

    public async Task<Comment?> GetByIdAsync(int id) =>
        await _context.Comments
            .Include(c => c.User)
            .SingleOrDefaultAsync(c => c.Id == id);

    public async Task<Page<Comment>> GetByPageAsync(int page, int threadId)
    {
        if (page <= 0)
            return new Page<Comment>(new List<Comment>(), true);

        var skipped = _context.Comments
            .Where(c => c.ThreadId == threadId)
            .OrderByDescending(c => c.CreatedDate)
            .Skip((page - 1) * Constants.PageSize);
        var isLast = Math.Ceiling((await skipped.CountAsync() - Constants.PageSize) / (float) Constants.PageSize) < 1;

        return new Page<Comment>(await skipped
            .Take(Constants.PageSize)
            .Include(c => c.User)
            .ToListAsync(), isLast);
    }

    public async Task<bool> Exists(int id) =>
        await _context.Comments.AnyAsync(c => c.Id == id);

    public async Task DeleteAsync(Comment entity)
    {
        _context.Comments.Remove(entity);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Comment entity)
    {
        _context.Comments.Update(entity);

        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(Comment entity)
    {
        _context.Comments.Add(entity);

        await _context.SaveChangesAsync();
    }
}