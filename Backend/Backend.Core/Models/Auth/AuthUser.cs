using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.Auth;

public class AuthUser : User
{
    [Required, MinLength(4), MaxLength(16)]
    public string Password { get; set; } = default!;
}