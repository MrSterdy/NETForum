using System.ComponentModel.DataAnnotations;

namespace NETForum.Models.Requests;

public class ThreadRequest
{
    [Required, MinLength(4), MaxLength(sbyte.MaxValue)]
    public string Title { get; set; } = default!;
    
    [Required, MinLength(4), MaxLength(short.MaxValue)]
    public string Content { get; set; } = default!;
}