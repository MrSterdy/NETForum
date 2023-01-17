using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.Comment;
using Backend.Core.Models.User.Auth;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.CommentsController;

public class CreateCommentsControllerTest : CommentsControllerTest
{
    public CreateCommentsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void Create_Ok()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        var user = thread.User;
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, true);
        var comment = new CommentRequest(thread.Id, thread.Content);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Auth/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, comment);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void Create_ThreadNotExist_NotFound()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, true);
        var comment = CommentGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Auth/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, comment);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void Create_InvalidModel_BadRequest()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        var user = thread.User;
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, true);
        var comment = new CommentRequest(thread.Id, "");
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Auth/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, comment);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}