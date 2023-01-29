namespace NETForum.Infrastructure.Mail;

public class MailSettings
{
    public string DisplayName { get; set; } = default!;
    public string From { get; set; } = default!;
    
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    
    public string Host { get; set; } = default!;
    public int Port { get; set; }
    
    public bool UseSsl { get; set; }
    
    public bool UseStartTls { get; set; }
}