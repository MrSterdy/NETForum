using System.ComponentModel.DataAnnotations;

namespace Backend.Database.Entities;

public class Thread : Entity
{
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    
    [Required, MinLength(4), MaxLength(sbyte.MaxValue)]
    public string Title { get; set; }
    [Required, MinLength(4), MaxLength(short.MaxValue)]
    public string Content { get; set; }

    public Thread(int userId, string title, string content)
    {
        UserId = userId;
        Title = title;
        Content = content;
    }

    public Thread(User user, string title, string content) : this(user.Id, title, content) =>
        User = user;
}