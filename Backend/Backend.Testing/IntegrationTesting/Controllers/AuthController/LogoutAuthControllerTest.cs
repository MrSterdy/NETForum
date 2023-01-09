using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.Auth;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.AuthController;

public class LogoutAuthControllerTest : AuthControllerTest
{
    public LogoutAuthControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void Logout_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new AuthUser { UserName = user.UserName!, Password = user.UserName! };
        
        // Act
        using var client = Factory.CreateClient();
        using var responseLogIn = await client.PostAsJsonAsync(Endpoint + "/login", loginUser);
        using var responseLogOut = await client.PostAsync(Endpoint + "/logout", null);
        
        // Assert
        responseLogIn.EnsureSuccessStatusCode();
        responseLogOut.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void Logout_UserNotLoggedIn_NotFound()
    {
        // Arrange

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsync(Endpoint + "/logout", null);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}