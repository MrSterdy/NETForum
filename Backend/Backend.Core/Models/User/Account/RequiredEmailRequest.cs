using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public class RequiredEmailRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;
}