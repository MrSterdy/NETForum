using Backend.Database.Entities;
using Thread = Backend.Database.Entities.Thread;

namespace Backend.Database.Repositories;

public interface IThreadRepository : IRepository<Thread>
{
    Task<EntityPage<Thread>> GetByPageAsync(int page);

    Task<EntityPage<Thread>> GetByUserIdAsync(int userId, int page);
}