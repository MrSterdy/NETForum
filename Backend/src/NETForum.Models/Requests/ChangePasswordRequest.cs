using System.ComponentModel.DataAnnotations;

namespace NETForum.Models.Requests;

public class ChangePasswordRequest
{
    public string? Password { get; set; }

    [Required, MinLength(4), MaxLength(16)]
    public string NewPassword { get; set; } = default!;
}