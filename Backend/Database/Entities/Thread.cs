using System.ComponentModel.DataAnnotations;

namespace Backend.Database.Entities;

public class Thread : Entity
{
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    [Required, MinLength(4), MaxLength(sbyte.MaxValue)]
    public string Name { get; set; }
    [Required, MinLength(4), MaxLength(short.MaxValue)]
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