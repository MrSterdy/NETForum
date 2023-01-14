using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Auth;

public record LoginUserRequest(
    [Required] string UserName, 
    [Required] string Password, 
    [Required] bool RememberMe
);