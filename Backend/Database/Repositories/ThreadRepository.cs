using Backend.Database.Entities;
using Thread = Backend.Database.Entities.Thread;

using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Repositories;

public class ThreadRepository : IThreadRepository
{
    private readonly Context _context;

    public ThreadRepository(Context context) =>
        _context = context;

    public async Task<IEnumerable<Thread>> GetAllAsync() =>
        await _context.Threads.Include("User").ToListAsync();

    public async Task<Thread?> GetByIdAsync(int id) =>
        await _context.Threads.Include("User").SingleOrDefaultAsync(t => t.Id == id);

    public async Task<EntityPage<Thread>> GetByPageAsync(int page)
    {
        if (page <= 0)
            return new EntityPage<Thread>(new List<Thread>(), true);

        var skipped = _context.Threads.Skip((page - 1) * Page.Capacity);
        var isLast = Math.Ceiling((skipped.Count() - Page.Capacity) / (float) Page.Capacity) < 1;

        return new EntityPage<Thread>(await skipped.Take(10).Include("User").ToListAsync(), isLast);
    }

    public async Task<EntityPage<Thread>> GetByUserIdAsync(int userId, int page)
    {
        if (page <= 0)
            return new EntityPage<Thread>(new List<Thread>(), true);

        var skipped = _context.Threads.Where(t => t.UserId == userId).Skip((page - 1) * Page.Capacity);
        var isLast = Math.Ceiling((skipped.Count() - Page.Capacity) / (float) Page.Capacity) < 1;

        return new EntityPage<Thread>(await skipped.Take(10).Include("User").ToListAsync(), isLast);
    }

    public async void DeleteAsync(Thread entity)
    {
        _context.Threads.Remove(entity);
        
        await _context.SaveChangesAsync();
    }
    
    public async void UpdateAsync(Thread entity)
    {
        _context.Threads.Update(entity);
        
        await _context.SaveChangesAsync();
    }

    public async void AddAsync(Thread entity)
    {
        _context.Threads.Add(entity);

        await _context.SaveChangesAsync();
    }
}