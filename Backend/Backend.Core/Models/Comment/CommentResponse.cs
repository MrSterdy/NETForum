using Backend.Core.Models.User;

namespace Backend.Core.Models.Comment;

public class CommentResponse
{
    public int Id { get; set; }
    
    public DateTimeOffset CreatedDate { get; set; }
    
    public UserResponse? User { get; set; }
    
    public int ThreadId { get; set; }
    
    public string? Content { get; set; }
}