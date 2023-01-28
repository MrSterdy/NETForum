namespace Backend.Core.Models.User.Account;

public class AccountResponse : UserResponse
{
    public string? Email { get; set; }
    
    public bool Confirmed { get; set; }
}