using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public record SignupRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(4), MaxLength(16)] string UserName,
    [Required, MinLength(4), MaxLength(16)] string Password
);