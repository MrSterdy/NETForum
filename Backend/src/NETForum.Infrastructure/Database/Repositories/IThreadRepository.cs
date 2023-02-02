using NETForum.Infrastructure.Database.Entities;
using Thread = NETForum.Infrastructure.Database.Entities.Thread;

namespace NETForum.Infrastructure.Database.Repositories;

public interface IThreadRepository : IRepository<Thread>
{
    Task<Page<Thread>> SearchAsync(int page, int[] tagIds, int? userId, string? title);
}