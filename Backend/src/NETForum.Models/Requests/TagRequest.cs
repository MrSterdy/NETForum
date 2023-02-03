using System.ComponentModel.DataAnnotations;

namespace NETForum.Models.Requests;

public class TagRequest
{
    [Required, MinLength(2), MaxLength(16)]
    public string Name { get; set; } = default!;
}