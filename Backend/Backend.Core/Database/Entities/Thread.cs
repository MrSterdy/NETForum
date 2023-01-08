using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace Backend.Core.Database.Entities;

public class Thread
{
    [Key]
    public int Id { get; private set; }
    
    [Required]
    public int UserId { get; set; }
    public IdentityUser<int> User { get; set; } = default!;
    
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

    public Thread(IdentityUser<int> user, string title, string content) : this(user.Id, title, content) =>
        User = user;
}