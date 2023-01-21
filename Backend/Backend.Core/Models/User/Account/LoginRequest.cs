using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public record LoginRequest(
    [Required] string UserName, 
    [Required] string Password, 
    [Required] bool RememberMe
);