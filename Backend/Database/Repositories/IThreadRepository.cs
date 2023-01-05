using Thread = Backend.Database.Entities.Thread;

namespace Backend.Database.Repositories;

public interface IThreadRepository : IRepository<Thread>
{
    Task<IEnumerable<Thread>> GetByPageAsync(int page);

    Task<IEnumerable<Thread>> GetByUserIdAsync(int userId, int page);
}