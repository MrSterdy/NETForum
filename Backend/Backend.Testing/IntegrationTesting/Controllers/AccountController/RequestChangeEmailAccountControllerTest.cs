using System.Net.Http.Json;

using Backend.Core.Models.User.Account;

using Bogus;

namespace Backend.Testing.IntegrationTesting.Controllers.AccountController;

public class RequestChangeEmailAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/ChangeEmail";

    private readonly Faker<RequiredEmailRequest> _generator = new Faker<RequiredEmailRequest>()
        .CustomInstantiator(faker => new RequiredEmailRequest(faker.Internet.Email()));

    public RequestChangeEmailAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void RequestChangeEmail_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest(user.UserName!, user.UserName!, true);
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