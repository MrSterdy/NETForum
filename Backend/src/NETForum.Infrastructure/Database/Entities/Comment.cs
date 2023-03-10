using System.ComponentModel.DataAnnotations;

using NETForum.Infrastructure.Identity;

namespace NETForum.Infrastructure.Database.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? ModifiedDate { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public ApplicationUser User { get; set; } = default!;
    
    [Required]
    public int ThreadId { get; set; }
    public Thread Thread { get; set; } = default!;

    [Required, MinLength(4), MaxLength(short.MaxValue)]
    public string Content { get; set; } = default!;
}