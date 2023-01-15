using Backend.Core.Models.Comment;
using Backend.Core.Models.User;

namespace Backend.Core.Models.Thread;

public record ThreadResponse(
    int Id, 
    UserResponse User,
    string Title, 
    string Content,
    IEnumerable<CommentResponse> Comments
);