using NETForum.Models.Requests;

using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

namespace NETForum.IntegrationTests.Controllers.AccountController;

public class LogoutAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Logout";

    public LogoutAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void Logout_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        
        // Act
        using var client = Factory.CreateClient();
        using var responseLogIn = await client.PostAsJsonAsync(base.Endpoint + "/Login", loginUser);
        using var responseLogOut = await client.PostAsync(Endpoint, null);
        
        // Assert
        responseLogIn.EnsureSuccessStatusCode();
        responseLogOut.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void Logout_UserNotLoggedIn_Unauthorized()
    {
        // Arrange

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsync(Endpoint, null);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}