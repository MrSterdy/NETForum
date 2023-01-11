namespace Backend.Core.Models.User;

public class UserResponse
{
    public int Id { get; set; }

    public string Email { get; set; }
    
    public string UserName { get; set; }

    public bool Confirmed { get; set; }

    public UserResponse(int id, string email, string userName, bool confirmed)
    {
        Id = id;
        Email = email;
        UserName = userName;
        Confirmed = confirmed;
    }
}