using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.User.Account;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.UsersController;

public class BanByIdUsersControllerTest : UsersControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Block";
    
    public BanByIdUsersControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task BanById_Ok()
    {
        // Arrange
        var target = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var user = await Factory.DbManager.Seeder.SeedAdminUserAsync();
        var login = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        
        // Act
        using var client = Factory.CreateClient();
        using var loginResponse = await client.PostAsJsonAsync("Api/Account/Login", login);
        using var blockResponse = await client.PostAsync(Endpoint + $"/{target.Id}", null);
        
        // Assert
        loginResponse.EnsureSuccessStatusCode();
        blockResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task BanById_NotFound()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedAdminUserAsync();
        var login = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        
        // Act
        using var client = Factory.CreateClient();
        using var loginResponse = await client.PostAsJsonAsync("Api/Account/Login", login);
        using var blockResponse = await client.PostAsync(Endpoint + "/0", null);
        
        // Assert
        loginResponse.EnsureSuccessStatusCode();
        blockResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}