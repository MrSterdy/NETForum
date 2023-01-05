using Backend.Database.Entities;

using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Context _context;

    public UserRepository(Context context) =>
        _context = context;

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _context.Users.ToListAsync();

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users.FindAsync(id);

    public async void DeleteAsync(User entity)
    {
        _context.Users.Remove(entity);
        
        await _context.SaveChangesAsync();
    }
    
    public async void UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
        
        await _context.SaveChangesAsync();
    }

    public async void AddAsync(User entity)
    {
        _context.Users.Add(entity);

        await _context.SaveChangesAsync();
    }
}