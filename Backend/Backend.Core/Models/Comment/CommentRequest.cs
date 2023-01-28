using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.Comment;

public class CommentRequest
{
    [Required, MinLength(4), MaxLength(short.MaxValue)]
    public string Content { get; set; } = default!;
}