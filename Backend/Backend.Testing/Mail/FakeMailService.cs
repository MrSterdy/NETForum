using Backend.Core.Mail;

namespace Backend.Testing.Mail;

public class FakeMailService : IMailService
{
    public Task SendMailAsync(string email, string subject, string body) =>
        Task.CompletedTask;
}