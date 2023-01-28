using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public class SignupRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;
    
    [Required, MinLength(4), MaxLength(16)]
    public string UserName { get; set; } = default!;
    
    [Required, MinLength(4), MaxLength(16)]
    public string Password { get; set; } = default!;
}