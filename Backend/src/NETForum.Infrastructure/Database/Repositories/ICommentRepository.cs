using NETForum.Infrastructure.Database.Entities;

namespace NETForum.Infrastructure.Database.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<Page<Comment>> GetByPageAsync(int page, int threadId);

    Task<int> GetCountByThreadIdAsync(int threadId);
}