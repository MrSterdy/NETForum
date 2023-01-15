using Backend.Core.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Database.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly Context _context;

    public CommentRepository(Context context) =>
        _context = context;

    public async Task<IEnumerable<Comment>> GetAllAsync() =>
        await _context.Comments.Include("User").Include("Thread").ToListAsync();

    public async Task<Comment?> GetByIdAsync(int id) =>
        await _context.Comments.Include("User").Include("Thread").SingleOrDefaultAsync(c => c.Id == id);

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