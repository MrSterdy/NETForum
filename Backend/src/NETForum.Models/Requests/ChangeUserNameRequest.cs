using System.ComponentModel.DataAnnotations;

namespace NETForum.Models.Requests;

public class ChangeUserNameRequest
{
    [Required, MinLength(4), MaxLength(16)]
    public string UserName { get; set; } = default!;
}