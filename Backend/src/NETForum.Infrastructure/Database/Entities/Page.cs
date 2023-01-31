namespace NETForum.Infrastructure.Database.Entities;

public class Page<T>
{
    public static Page<T> Empty => new (Enumerable.Empty<T>(), true);

    public IEnumerable<T> Items { get; set; }
    
    public bool IsLast { get; set; }

    public Page(IEnumerable<T> items, bool isLast)
    {
        Items = items;
        IsLast = isLast;
    }
}