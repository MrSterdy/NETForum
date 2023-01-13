using Backend.Core.Models.User;

namespace Backend.Core.Models.Thread;

public record ThreadResponse(int Id, UserResponse User, string Title, string Content);