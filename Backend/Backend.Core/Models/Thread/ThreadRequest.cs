using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Models.Thread;

public record ThreadRequest(
    [Required, MinLength(4), MaxLength(sbyte.MaxValue)] string Title,
    [Required, MinLength(4), MaxLength(short.MaxValue)] string Content
);