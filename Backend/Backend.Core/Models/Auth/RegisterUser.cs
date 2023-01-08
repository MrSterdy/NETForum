using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.Auth;

public class RegisterUser : AuthUser
{
    [Required, EmailAddress]
    public override string? Email { get; set; }
}