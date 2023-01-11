namespace Backend.Core.Models;

public class Page<T>
{
    public IEnumerable<T> Items { get; }

    public bool IsLast { get; }

    public Page(IEnumerable<T> items, bool isLast)
    {
        Items = items;
        IsLast = isLast;
    }
}