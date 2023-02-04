using System.ComponentModel.DataAnnotations;

using NETForum.Models.Requests.RequestValidation;

namespace NETForum.Models.Requests;

public class ChangeUserNameRequest
{
    [Required, MinLength(4), MaxLength(16), UniqueUserName]
    public string UserName { get; set; } = default!;
}