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

    public async Task<Page<Thread>> GetByPageAsync(int page)
    {
        if (page <= 0)
            return new Page<Thread>(new List<Thread>(), true);

        var skipped = _context.Threads
            .OrderByDescending(t => t.CreatedDate)
            .Skip((page - 1) * Constants.PageSize);
        var isLast = Math.Ceiling((await skipped.CountAsync() - Constants.PageSize) / (float) Constants.PageSize) < 1;

        return new Page<Thread>(await skipped
            .Take(Constants.PageSize)
            .Include(t => t.User)
            .Include(t => t.Tags)
                .ThenInclude(t => t.Tag)
            .ToListAsync(), isLast);
    }

    public async Task<Page<Thread>> GetByUserIdAsync(int userId, int page)
    {
        if (page <= 0)
            return Page<Thread>.Empty;

        var skipped = _context.Threads
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedDate)
            .Skip((page - 1) * Constants.PageSize);
        var isLast = Math.Ceiling((await skipped.CountAsync() - Constants.PageSize) / (float) Constants.PageSize) < 1;

        return new Page<Thread>(await skipped
            .Take(Constants.PageSize)
            .Include(t => t.User)
            .Include(t => t.Tags)
                .ThenInclude(t => t.Tag)
            .ToListAsync(), isLast);
    }

    public async Task<Page<Thread>> SearchAsync(string? title, string[]? tags, int page)
    {
        if (page <= 0 || (title is null && tags is null))
            return Page<Thread>.Empty;

        var threads = _context.Threads.AsQueryable();

        if (title is not null)
            threads = threads.Where(t => t.Title.Contains(title));
        
        if (tags is not null)
            threads = threads
                .Where(t => !t.Tags.All(threadTag => tags.Contains(threadTag.Tag.Name))); // ???

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