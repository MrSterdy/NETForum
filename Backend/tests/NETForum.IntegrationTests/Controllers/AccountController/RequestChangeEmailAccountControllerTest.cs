using System.Net.Http.Json;

using NETForum.Models.Requests;

using Bogus;

namespace NETForum.IntegrationTests.Controllers.AccountController;

public class RequestChangeEmailAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/ChangeEmail";

    private readonly Faker<ChangeEmailRequest> _generator = new Faker<ChangeEmailRequest>()
        .RuleFor(r => r.Email, faker => faker.Internet.Email());

    public RequestChangeEmailAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void RequestChangeEmail_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        var emailRequest = _generator.Generate();

        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(base.Endpoint + "/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(
            Endpoint + $"?callbackUrl={new Faker().Internet.Url()}",
            emailRequest
        );
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
}