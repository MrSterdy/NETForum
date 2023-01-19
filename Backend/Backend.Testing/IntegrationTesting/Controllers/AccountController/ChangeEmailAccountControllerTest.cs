using System.Net.Http.Json;

using Backend.Core.Models.User.Account;
using Backend.Core.Models.User.Auth;

using Bogus;

namespace Backend.Testing.IntegrationTesting.Controllers.AccountController;

public class ChangeEmailAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Change/Email";

    private readonly Faker<ChangeEmailRequest> _generator = new Faker<ChangeEmailRequest>()
        .CustomInstantiator(faker => new ChangeEmailRequest(faker.Internet.Email()));

    public ChangeEmailAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void ChangeEmail_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, true);
        var emailRequest = _generator.Generate();

        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Auth/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(
            Endpoint + $"?clientUrl={new Faker().Internet.Url()}",
            emailRequest
        );
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
}