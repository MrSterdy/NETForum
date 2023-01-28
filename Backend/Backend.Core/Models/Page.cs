namespace Backend.Core.Models;

public class Page<T>
{
    public IEnumerable<T> Items { get; set; }
    
    public bool IsLast { get; set; }

    public Page(IEnumerable<T> items, bool isLast)
    {
        Items = items;
        IsLast = isLast;
    }
}