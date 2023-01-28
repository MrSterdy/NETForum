using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.Thread;
using Backend.Core.Models.User.Account;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.ThreadsController;

public class CreateThreadsControllerTest : ThreadsControllerTest
{
    public CreateThreadsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void Create_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        var thread = ThreadGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Account/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, thread);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void Create_InvalidModel_BadRequest()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        var thread = new ThreadRequest();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Account/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, thread);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}