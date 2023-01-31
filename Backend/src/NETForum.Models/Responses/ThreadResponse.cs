namespace NETForum.Models.Responses;

public class ThreadResponse
{
    public int Id { get; set; }
    
    public DateTimeOffset CreatedDate { get; set; }
    
    public UserResponse? User { get; set; }

    public string? Title { get; set; }
    
    public string? Content { get; set; }
    
    public int Views { get; set; }

    public int CommentCount { get; set; }
    
    public IEnumerable<TagResponse>? Tags { get; set; }
}