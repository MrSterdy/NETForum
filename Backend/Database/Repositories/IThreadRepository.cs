using Thread = Backend.Database.Entities.Thread;

namespace Backend.Database.Repositories;

public interface IThreadRepository : IRepository<Thread>
{
    Task<Page<Thread>> GetByPageAsync(int page);

    Task<Page<Thread>> GetByUserIdAsync(int userId, int page);
}