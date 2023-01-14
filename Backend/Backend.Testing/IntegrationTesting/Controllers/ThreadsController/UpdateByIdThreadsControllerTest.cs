using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.User.Auth;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.ThreadsController;

public class UpdateByIdThreadsControllerTest : ThreadsControllerTest
{
    public UpdateByIdThreadsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void UpdateById_Ok()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        var user = thread.User;
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, false);
        var newThread = ThreadGenerator.Generate();

        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Auth/Login", loginUser);
        using var secondResponse = await client.PutAsJsonAsync(Endpoint + $"/{thread.Id}", newThread);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void UpdateById_ThreadNotExist_NotFound()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, false);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Auth/Login", loginUser);
        using var secondResponse = await client.DeleteAsync(Endpoint + "/0");
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async void UpdateById_UserNotOwner_Forbidden()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, false);
        var newThread = ThreadGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Auth/Login", loginUser);
        using var secondResponse = await client.PutAsJsonAsync(Endpoint + $"/{thread.Id}", newThread);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}