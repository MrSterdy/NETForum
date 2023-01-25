namespace Backend.Core.Models.User;

public record UserResponse(
    int Id,
    string UserName,
    bool IsAdmin
);