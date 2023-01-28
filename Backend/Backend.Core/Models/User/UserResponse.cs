namespace Backend.Core.Models.User;

public class UserResponse
{
    public int Id { get; set; }
    
    public string? UserName { get; set; }
    
    public bool Enabled { get; set; }
    
    public bool Admin { get; set; }
}