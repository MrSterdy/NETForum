using NETForum.Infrastructure.Identity;
using NETForum.Infrastructure.Database.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NETForum.Infrastructure.Database.Repositories;

public interface IUserRepository : IRepository<ApplicationUser>
{
    public Task<Page<ApplicationUser>> SearchAsync(string username, int page);
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

    public async Task<Page<ApplicationUser>> SearchAsync(string username, int page)
    {
        if (page <= 0)
            return Page<ApplicationUser>.Empty;

        var skipped = _manager.Users
            .Where(u => u.UserName!.Contains(username))
            .Skip((page - 1) * Constants.PageSize);
        var isLast = Math.Ceiling((await skipped.CountAsync() - Constants.PageSize) / (float) Constants.PageSize) < 1;

        return new Page<ApplicationUser>(await skipped
            .Take(Constants.PageSize)
            .ToListAsync(), isLast);
    }

    public async Task<bool> Exists(int id) =>
        await _manager.Users.AnyAsync(u => u.Id == id);

    public async Task DeleteAsync(ApplicationUser entity) =>
        await _manager.DeleteAsync(entity);

    public async Task UpdateAsync(ApplicationUser entity) =>
        await _manager.UpdateAsync(entity);

    public async Task AddAsync(ApplicationUser entity) =>
        await _manager.CreateAsync(entity);
}