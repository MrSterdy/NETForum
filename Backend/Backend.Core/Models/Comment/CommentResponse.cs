using Backend.Core.Models.User;

namespace Backend.Core.Models.Comment;

public record CommentResponse(
    int Id,
    UserResponse User,
    int ThreadId,
    string Content
);