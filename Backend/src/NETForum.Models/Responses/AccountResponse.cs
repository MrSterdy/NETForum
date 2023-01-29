namespace NETForum.Models.Responses;

public class AccountResponse : UserResponse
{
    public string? Email { get; set; }
    
    public bool EmailConfirmed { get; set; }
}