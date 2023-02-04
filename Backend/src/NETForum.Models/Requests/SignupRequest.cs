using System.ComponentModel.DataAnnotations;

using NETForum.Models.Requests.RequestValidation;

namespace NETForum.Models.Requests;

public class SignupRequest
{
    [Required, EmailAddress, UniqueEmailAddress]
    public string Email { get; set; } = default!;
    
    [Required, MinLength(4), MaxLength(16), UniqueUserName]
    public string UserName { get; set; } = default!;
    
    [Required, MinLength(4), MaxLength(16)]
    public string Password { get; set; } = default!;
}