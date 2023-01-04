namespace Backend.Database.Entities;

public class Thread : Entity
{
    public long UserId { get; private set; }
    public User User { get; private set; } = default!;
    
    public string Name { get; set; }
    public string Content { get; set; }

    public Thread(long userId, string name, string content)
    {
        UserId = userId;
        Name = name;
        Content = content;
    }
}