using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public class ChangePasswordRequest
{
    public string? Password { get; set; }

    [Required, MinLength(4), MaxLength(16)]
    public string NewPassword { get; set; } = default!;
}