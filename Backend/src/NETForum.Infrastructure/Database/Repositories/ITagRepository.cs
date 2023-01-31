using NETForum.Infrastructure.Database.Entities;

namespace NETForum.Infrastructure.Database.Repositories;

public interface ITagRepository : IRepository<Tag>
{
    Task<Page<Tag>> GetByPageAsync(int page);
}