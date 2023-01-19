using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Auth;

public record SignupUserRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(4), MaxLength(16)] string UserName,
    [Required, MinLength(4), MaxLength(16)] string Password
);