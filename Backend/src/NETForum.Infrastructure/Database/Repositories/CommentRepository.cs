using NETForum.Infrastructure.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace NETForum.Infrastructure.Database.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context) =>
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

    public async Task<int> GetCountByThreadIdAsync(int threadId) =>
        await _context.Comments.Where(c => c.ThreadId == threadId).CountAsync();

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