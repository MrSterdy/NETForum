using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models;
using Backend.Core.Models.Auth;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.UserController;

public class GetCurrentUserControllerTest : UserControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Current";

    public GetCurrentUserControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void GetCurrent_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var authUser = new AuthUser { UserName = user.UserName!, Password = user.UserName! };
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Auth/Login", authUser);
        using var secondResponse = await client.GetAsync(Endpoint);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
        
        var result = await ParseResponse<User>(secondResponse);
        result.UserName.Should().Be(user.UserName);
        result.Email.Should().Be(user.Email);
    }
    
    [Fact]
    public async void GetCurrent_NotFound()
    {
        // Arrange

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}