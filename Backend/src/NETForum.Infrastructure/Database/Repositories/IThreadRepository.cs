using NETForum.Infrastructure.Database.Entities;
using Thread = NETForum.Infrastructure.Database.Entities.Thread;

using Microsoft.EntityFrameworkCore;

namespace NETForum.Infrastructure.Database.Repositories;

public interface IThreadRepository : IRepository<Thread>
{
    Task<Page<Thread>> SearchAsync(int page, int[] tagIds, int? userId, string? title);
}

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

    public async Task<Page<Thread>> SearchAsync(int page, int[] tagIds, int? userId, string? title)
    {
        if (page <= 0)
            return Page<Thread>.Empty;

        var threads = _context.Threads.AsQueryable();

        if (userId is not null)
            threads = threads.Where(t => t.UserId == userId);

        if (title is not null)
            threads = threads.Where(t => t.Title.Contains(title));

        if (tagIds.Length != 0)
        {
            var list = await threads
                .Include(t => t.Tags)
                    .ThenInclude(t => t.Tag)
                .ToListAsync();

            threads = list
                .Where(t => tagIds.All(tagId => t.Tags.Any(threadTag => threadTag.TagId == tagId)))
                .AsQueryable();
        }

        threads = threads
            .OrderByDescending(t => t.CreatedDate)
            .Skip((page - 1) * Constants.PageSize);
        var isLast = Math.Ceiling((threads.Count() - Constants.PageSize) / (float) Constants.PageSize) < 1;

        return new Page<Thread>(threads
            .Take(Constants.PageSize)
            .Include(t => t.User)
            .Include(t => t.Tags)
                .ThenInclude(t => t.Tag)
            .ToList(), isLast);
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