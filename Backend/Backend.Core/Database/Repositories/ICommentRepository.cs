using Backend.Core.Database.Entities;
using Backend.Core.Models;

namespace Backend.Core.Database.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<Page<Comment>> GetByPageAsync(int page, int threadId);
}