namespace NETForum.Models.Responses;

public class UserResponse
{
    public int Id { get; set; }
    
    public string? UserName { get; set; }
    
    public bool Banned { get; set; }
    
    public bool Admin { get; set; }
}