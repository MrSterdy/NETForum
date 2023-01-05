namespace Backend.Database.Entities;

public abstract class Page
{
    public const int Capacity = 10;
    
    public IEnumerable<Entity> Entities { get; set; }
    
    public bool IsLast { get; set; }

    public Page(IEnumerable<Entity> entities, bool isLast)
    {
        Entities = entities;
        IsLast = isLast;
    }
}