using Backend.Core.Models;
using Thread = Backend.Core.Database.Entities.Thread;

using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database.Repositories;

public class ThreadRepository : IThreadRepository
{
    private readonly Context _context;

    public ThreadRepository(Context context) =>
        _context = context;

    public async Task<IEnumerable<Thread>> GetAllAsync() =>
        await _context.Threads.Include("User").ToListAsync();

    public async Task<Thread?> GetByIdAsync(int id) =>
        await _context.Threads.Include("User").SingleOrDefaultAsync(t => t.Id == id);

    public async Task<Page<Thread>> GetByPageAsync(int page)
    {
        if (page <= 0)
            return new Page<Thread>(new List<Thread>(), true);

        var skipped = _context.Threads.Skip((page - 1) * Constants.PageSize);
        var isLast = Math.Ceiling((skipped.Count() - Constants.PageSize) / (float) Constants.PageSize) < 1;

        return new Page<Thread>(await skipped.Take(10).Include("User").ToListAsync(), isLast);
    }

    public async Task<Page<Thread>> GetByUserIdAsync(int userId, int page)
    {
        if (page <= 0)
            return new Page<Thread>(new List<Thread>(), true);

        var skipped = _context.Threads.Where(t => t.UserId == userId).Skip((page - 1) * Constants.PageSize);
        var isLast = Math.Ceiling((skipped.Count() - Constants.PageSize) / (float) Constants.PageSize) < 1;

        return new Page<Thread>(await skipped.Take(10).Include("User").ToListAsync(), isLast);
    }

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