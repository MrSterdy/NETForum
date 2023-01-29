using System.ComponentModel.DataAnnotations;

namespace NETForum.Models.Requests;

public class CommentRequest
{
    [Required, MinLength(4), MaxLength(short.MaxValue)]
    public string Content { get; set; } = default!;
}