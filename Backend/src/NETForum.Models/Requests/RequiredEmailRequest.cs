using System.ComponentModel.DataAnnotations;

namespace NETForum.Models.Requests;

public class RequiredEmailRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;
}