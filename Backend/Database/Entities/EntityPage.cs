namespace Backend.Database.Entities;

public class EntityPage<T> : Page where T : Entity
{
    public EntityPage(IEnumerable<T> entities, bool isLast) : base(entities, isLast)
    {
    }
}