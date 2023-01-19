using System.Net.Http.Json;

using Backend.Core.Models.User.Auth;
using Bogus;

namespace Backend.Testing.IntegrationTesting.Controllers.AccountController;

public class ChangePasswordAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Change/Password";

    public ChangePasswordAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void ChangePassword_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, true);

        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Auth/Login", loginUser);
        using var secondResponse = await client.PostAsync(Endpoint + $"?clientUrl={new Faker().Internet.Url()}", null);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
}