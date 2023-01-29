namespace NETForum.Infrastructure.Database.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(int id);

    Task<bool> Exists(int id);

    Task DeleteAsync(T entity);

    Task UpdateAsync(T entity);

    Task AddAsync(T entity);
}