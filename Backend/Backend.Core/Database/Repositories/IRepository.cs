namespace Backend.Core.Database.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(int id);

    void DeleteAsync(T entity);

    void UpdateAsync(T entity);

    void AddAsync(T entity);
}