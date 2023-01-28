using System.Net.Http.Json;

using Backend.Core.Models.User.Account;

using Bogus;

namespace Backend.Testing.IntegrationTesting.Controllers.AccountController;

public class ChangePasswordAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Password";

    public ChangePasswordAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void ChangePassword_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        var changePassword = new ChangePasswordRequest
        {
            Password = user.UserName,
            NewPassword = new Faker().Internet.Password()
        };

        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(base.Endpoint + "/Login", loginUser);
        using var secondResponse = await client.PutAsJsonAsync(Endpoint, changePassword);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
}