using NETForum.Infrastructure.Database.Entities;
using Thread = NETForum.Infrastructure.Database.Entities.Thread;

using Microsoft.EntityFrameworkCore;

namespace NETForum.Infrastructure.Database.Repositories;

public class ThreadRepository : IThreadRepository
{
    private readonly ApplicationDbContext _context;

    public ThreadRepository(ApplicationDbContext context) =>
        _context = context;

    public async Task<IEnumerable<Thread>> GetAllAsync() =>
        await _context.Threads
            .Include(t => t.User)
            .ToListAsync();

    public async Task<Thread?> GetByIdAsync(int id) =>
        await _context.Threads
            .Include(t => t.User)
            .Include(t => t.Tags)
                .ThenInclude(t => t.Tag)
            .SingleOrDefaultAsync(t => t.Id == id);

    public async Task<Page<Thread>> SearchAsync(int? userId, string? title, int[]? tagIds, int page)
    {
        if (page <= 0 || (userId is null && title is null && tagIds is null))
            return Page<Thread>.Empty;

        var threads = _context.Threads.AsQueryable();

        if (userId is not null)
            threads = threads.Where(t => t.UserId == userId);

        if (title is not null)
            threads = threads.Where(t => t.Title.Contains(title));
        
        if (tagIds is not null)
            threads = threads
                .Where(t => !t.Tags.All(threadTag => tagIds.Contains(threadTag.TagId))); // ???

        threads = threads
            .OrderByDescending(t => t.CreatedDate)
            .Skip((page - 1) * Constants.PageSize);
        var isLast = Math.Ceiling((await threads.CountAsync() - Constants.PageSize) / (float) Constants.PageSize) < 1;

        return new Page<Thread>(await threads
            .Take(Constants.PageSize)
            .Include(t => t.User)
            .Include(t => t.Tags)
                .ThenInclude(t => t.Tag)
            .ToListAsync(), isLast);
    }

    public async Task<bool> Exists(int id) =>
        await _context.Threads.AnyAsync(t => t.Id == id);

    public async Task DeleteAsync(Thread entity)
    {
        _context.Threads.Remove(entity);
        
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Thread entity)
    {
        _context.Threads.Update(entity);
        
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(Thread entity)
    {
        _context.Threads.Add(entity);

        await _context.SaveChangesAsync();
    }
}