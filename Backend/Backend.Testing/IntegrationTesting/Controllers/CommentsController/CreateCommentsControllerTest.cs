using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.Comment;
using Backend.Core.Models.User.Account;

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
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        var comment = CommentGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Account/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(
            Endpoint + $"?threadId={thread.Id}",
            comment
        );
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void Create_ThreadNotExist_NotFound()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        var comment = CommentGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Account/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(
            Endpoint + "?threadId=0",
            comment
        );
        
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
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        var comment = new CommentRequest();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Account/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(
            Endpoint + $"?threadId={thread.Id}",
            comment
        );
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}