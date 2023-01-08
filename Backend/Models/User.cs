using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class User
{
    [Required, MinLength(4), MaxLength(16)]
    public string UserName { get; set; } = default!;
    
    [EmailAddress]
    public virtual string? Email { get; set; }
}