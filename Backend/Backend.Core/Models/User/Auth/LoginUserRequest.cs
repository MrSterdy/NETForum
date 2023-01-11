using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Auth;

public class LoginUserRequest
{
    [Required] 
    public string UserName { get; set; } = default!;
    
    [Required]
    public string Password { get; set; } = default!;
}