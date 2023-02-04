using System.ComponentModel.DataAnnotations;

namespace NETForum.Models.Requests;

public class ResetPasswordRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;
}