using NETForum.Infrastructure.Database.Entities;
using Thread = NETForum.Infrastructure.Database.Entities.Thread;

namespace NETForum.Infrastructure.Database.Repositories;

public interface IThreadRepository : IRepository<Thread>
{
    Task<Page<Thread>> GetByPageAsync(int page);

    Task<Page<Thread>> GetByUserIdAsync(int userId, int page);

    Task<Page<Thread>> SearchAsync(string? title, string[]? tags, int page);
}