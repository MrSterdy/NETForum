namespace Backend.Core.Models.Comment;

public record CommentResponse(
    int Id,
    int UserId,
    int ThreadId,
    string Content
);