using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public class LoginRequest
{
    [Required]
    public string UserName { get; set; } = default!;
    
    [Required]
    public string Password { get; set; } = default!;
    
    public bool RememberMe { get; set; }
}