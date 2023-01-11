using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models;

public class User
{
    public int Id { get; set; }
    
    [Required, MinLength(4), MaxLength(16)]
    public string UserName { get; set; } = default!;

    [EmailAddress]
    public virtual string? Email { get; set; }
    
    public bool EmailConfirmed { get; set; }
}