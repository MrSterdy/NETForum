using System.ComponentModel.DataAnnotations;

using NETForum.Models.Requests.RequestValidation;

namespace NETForum.Models.Requests;

public class ChangeEmailRequest
{
    [Required, EmailAddress, UniqueEmailAddress]
    public string Email { get; set; } = default!;
}