using System.Net.Http.Json;

using Backend.Core.Models.User.Account;

using Bogus;

namespace Backend.Testing.IntegrationTesting.Controllers.AccountController;

public class RequestResetPasswordAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/ResetPassword";

    public RequestResetPasswordAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void RequestResetPassword_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest(user.UserName!, user.UserName!, true);
        var changePassword = new ChangePasswordRequest(user.UserName!, new Faker().Internet.Password());

        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(base.Endpoint + "/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(
            Endpoint + $"?callbackUrl={new Faker().Internet.Url()}",
            changePassword
        );
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
}