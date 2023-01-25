using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.User.Account;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.ThreadsController;

public class DeleteByIdThreadsControllerTest : ThreadsControllerTest
{
    public DeleteByIdThreadsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void DeleteById_Ok()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        var user = thread.User;
        var loginUser = new LoginRequest(user.UserName!, user.UserName!, false);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Account/Login", loginUser);
        using var secondResponse = await client.DeleteAsync(Endpoint + $"/{thread.Id}");
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void DeleteById_Admin_Ok()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        var user = await Factory.DbManager.Seeder.SeedAdminUserAsync();
        var loginUser = new LoginRequest(user.UserName!, user.UserName!, false);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Account/Login", loginUser);
        using var secondResponse = await client.DeleteAsync(Endpoint + $"/{thread.Id}");
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void DeleteById_ThreadNotExist_NotFound()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest(user.UserName!, user.UserName!, false);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Account/Login", loginUser);
        using var secondResponse = await client.DeleteAsync(Endpoint + "/0");
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async void DeleteById_UserNotOwner_Forbidden()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest(user.UserName!, user.UserName!, false);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Account/Login", loginUser);
        using var secondResponse = await client.DeleteAsync(Endpoint + $"/{thread.Id}");
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}