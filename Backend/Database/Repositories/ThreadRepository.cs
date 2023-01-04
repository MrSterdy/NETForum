using Thread = Backend.Database.Entities.Thread;

using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Repositories;

public class ThreadRepository : IThreadRepository
{
    private readonly Context _context;

    public ThreadRepository(Context context) =>
        _context = context;

    public async Task<IEnumerable<Thread>> GetAllAsync() =>
        await _context.Threads.ToListAsync();

    public async Task<Thread?> GetByIdAsync(long id) =>
        await _context.Threads.FindAsync(id);

    public async Task<IEnumerable<Thread>> GetByPageAsync(int page) =>
        page <= 0 ? new List<Thread>() : await _context.Threads.Skip((page - 1) * 10).Take(10).ToListAsync();

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