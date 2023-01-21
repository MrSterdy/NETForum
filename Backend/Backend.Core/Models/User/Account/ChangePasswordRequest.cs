using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public record ChangePasswordRequest(
    [Required] string Password,
    [MinLength(4), MaxLength(16)] string? NewPassword
);