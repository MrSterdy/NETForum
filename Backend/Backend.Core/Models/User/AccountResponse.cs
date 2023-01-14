namespace Backend.Core.Models.User;

public record AccountResponse(
    int Id, 
    string Email, 
    bool Confirmed, 
    string UserName
) : UserResponse(Id, Email, UserName);