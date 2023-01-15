using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.Comment;

public record CommentRequest(
    [Required] int ThreadId,
    [Required, MinLength(4), MaxLength(short.MaxValue)] string Content
);