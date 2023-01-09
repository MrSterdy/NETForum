using MimeKit;

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace Backend.Core.Mail;

public class MailService : IMailService
{
    private readonly MailSettings _settings;
    
    private readonly MailboxAddress _sender;

    public MailService(IOptions<MailSettings> options)
    {
        _settings = options.Value;

        _sender = new MailboxAddress(_settings.DisplayName, _settings.From);
    }

    public async Task SendMailAsync(string email, string subject, string body)
    {
        var mail = new MimeMessage();

        mail.From.Add(_sender);
        mail.To.Add(MailboxAddress.Parse(email));

        mail.Subject = subject;
        mail.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();

        using var smtp = new SmtpClient();
        
        if (_settings.UseSsl)
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect);
        else if (_settings.UseStartTls)
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
        
        await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);

        await smtp.SendAsync(mail);

        await smtp.DisconnectAsync(true);
    }
}