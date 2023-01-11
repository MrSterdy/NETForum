using Backend.Core.Models.User;

namespace Backend.Core.Models.Thread;

public class ThreadResponse
{
    public int Id { get; set; }

    public UserResponse User { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }

    public ThreadResponse(int id, UserResponse user, string title, string content)
    {
        Id = id;
        User = user;
        Title = title;
        Content = content;
    }
}