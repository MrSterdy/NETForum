using NETForum.Infrastructure.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace NETForum.Infrastructure.Database.Repositories;

public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context) =>
        _context = context;

    public async Task<IEnumerable<Tag>> GetAllAsync() =>
        await _context.Tags.ToListAsync();

    public async Task<Tag?> GetByIdAsync(int id) =>
        await _context.Tags.FindAsync(id);

    public async Task<Page<Tag>> GetByPageAsync(int page)
    {
        if (page <= 0)
            return Page<Tag>.Empty;

        var skipped = _context.Tags.Skip((page - 1) * Constants.PageSize);
        var isLast = Math.Ceiling((await skipped.CountAsync() - Constants.PageSize) / (float) Constants.PageSize) < 1;

        return new Page<Tag>(await skipped.Take(Constants.PageSize).ToListAsync(), isLast);
    }

    public async Task<bool> Exists(int id) =>
        await _context.Tags.AnyAsync(t => t.Id == id);

    public async Task DeleteAsync(Tag entity)
    {
        _context.Tags.Remove(entity);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tag entity)
    {
        _context.Tags.Update(entity);

        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(Tag entity)
    {
        _context.Tags.Add(entity);

        await _context.SaveChangesAsync();
    }
}