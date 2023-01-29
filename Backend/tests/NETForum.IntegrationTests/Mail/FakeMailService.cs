using NETForum.Infrastructure.Mail;

namespace NETForum.IntegrationTests.Mail;

public class FakeMailService : IMailService
{
    public Task SendMailAsync(string email, string subject, string body) =>
        Task.CompletedTask;
}