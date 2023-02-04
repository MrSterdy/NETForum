using System.ComponentModel.DataAnnotations;

using NETForum.Models.Requests.RequestValidation;

namespace NETForum.Models.Requests;

public class ThreadRequest
{
    [Required, MinLength(4), MaxLength(sbyte.MaxValue)]
    public string Title { get; set; } = default!;
    
    [Required, MinLength(4), MaxLength(short.MaxValue)]
    public string Content { get; set; } = default!;

    [UniqueTagIds]
    public int[] TagIds { get; set; } = Array.Empty<int>();
}