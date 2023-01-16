using Backend.Core.Models.User;

namespace Backend.Core.Models.Thread;

public record ThreadResponse(
    int Id, 
    DateTimeOffset CreatedDate,
    UserResponse User,
    string Title,
    string Content
);