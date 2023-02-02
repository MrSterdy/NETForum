using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETForum.Infrastructure.Identity;

namespace NETForum.Infrastructure.Database.Repositories;

public interface IUserRepository : IRepository<ApplicationUser>
{
}

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _manager;

    public UserRepository(UserManager<ApplicationUser> manager) =>
        _manager = manager;
    
    public async Task<IEnumerable<ApplicationUser>> GetAllAsync() =>
        await _manager.Users.ToListAsync();

    public async Task<ApplicationUser?> GetByIdAsync(int id) =>
        await _manager.FindByIdAsync(id.ToString());

    public async Task<bool> Exists(int id) =>
        await _manager.Users.AnyAsync(u => u.Id == id);

    public async Task DeleteAsync(ApplicationUser entity) =>
        await _manager.DeleteAsync(entity);

    public async Task UpdateAsync(ApplicationUser entity) =>
        await _manager.UpdateAsync(entity);

    public async Task AddAsync(ApplicationUser entity) =>
        await _manager.CreateAsync(entity);
}