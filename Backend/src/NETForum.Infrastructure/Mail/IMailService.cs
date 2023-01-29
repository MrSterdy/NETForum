namespace NETForum.Infrastructure.Mail;

public interface IMailService
{
    Task SendMailAsync(string email, string subject, string body);
}