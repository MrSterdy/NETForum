using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.User.Account;

public class ChangeUserNameRequest
{
    [Required, MinLength(4), MaxLength(16)]
    public string UserName { get; set; } = default!;
}