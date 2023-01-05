namespace Backend.Database.Entities;

public class Thread : Entity
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    public string Name { get; set; }
    public string Content { get; set; }

    public Thread(int userId, string name, string content)
    {
        UserId = userId;
        Name = name;
        Content = content;
    }

    public Thread(User user, string name, string content) : this(user.Id, name, content) =>
        User = user;
}