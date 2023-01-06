namespace Backend.Database.Entities;

public class Page<T> where T : Entity
{
    public IEnumerable<T> Items { get; }

    public bool IsLast { get; }

    public Page(IEnumerable<T> items, bool isLast)
    {
        Items = items;
        IsLast = isLast;
    }
}