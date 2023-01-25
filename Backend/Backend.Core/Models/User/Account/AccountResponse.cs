namespace Backend.Core.Models.User.Account;

public record AccountResponse(
    int Id, 
    string Email, 
    bool Confirmed, 
    string UserName,
    bool IsAdmin
) : UserResponse(Id, UserName, IsAdmin);