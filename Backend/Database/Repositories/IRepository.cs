using Backend.Database.Entities;

namespace Backend.Database.Repositories;

public interface IRepository<T> where T : Entity
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(int id);

    void DeleteAsync(T entity);

    void UpdateAsync(T entity);

    void AddAsync(T entity);
}