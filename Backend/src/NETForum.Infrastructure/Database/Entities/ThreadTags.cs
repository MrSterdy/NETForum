namespace NETForum.Infrastructure.Database.Entities;

public class ThreadTags
{
    public int ThreadId { get; set; }
    public Thread Thread { get; set; } = default!;
    
    public int TagId { get; set; }
    public Tag Tag { get; set; } = default!;
}