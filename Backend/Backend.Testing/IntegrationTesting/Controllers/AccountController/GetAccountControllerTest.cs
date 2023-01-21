using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.User.Account;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.AccountController;

public class GetAccountControllerTest : AccountControllerTest
{
    public GetAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void Get_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var authUser = new LoginRequest(user.UserName!, user.UserName!, true);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(Endpoint + "/Login", authUser);
        using var secondResponse = await client.GetAsync(Endpoint);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
        
        var result = await ParseResponse<AccountResponse>(secondResponse);
        result.UserName.Should().Be(user.UserName);
        result.Email.Should().Be(user.Email);
    }
    
    [Fact]
    public async void Get_Unauthorized()
    {
        // Arrange

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}