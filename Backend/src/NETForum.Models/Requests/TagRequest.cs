using System.ComponentModel.DataAnnotations;

using NETForum.Models.Requests.RequestValidation;

namespace NETForum.Models.Requests;

public class TagRequest
{
    [Required, MinLength(2), MaxLength(16), UniqueTagName]
    public string Name { get; set; } = default!;
}