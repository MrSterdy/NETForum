using System.Net;
using System.Net.Http.Json;

using NETForum.Models.Requests;

using FluentAssertions;

namespace NETForum.IntegrationTests.Controllers.CommentsController;

public class UpdateByIdCommentsControllerTest : CommentsControllerTest
{
    public UpdateByIdCommentsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void UpdateById_Ok()
    {
        // Arrange
        var comment = await Factory.DbManager.Seeder.SeedCommentAsync();
        var user = comment.User;
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        var newComment = CommentGenerator.Generate();

        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Account/Login", loginUser);
        using var secondResponse = await client.PutAsJsonAsync(Endpoint + $"/{comment.Id}", newComment);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void UpdateById_CommentNotExist_NotFound()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        var newComment = CommentGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Account/Login", loginUser);
        using var secondResponse = await client.PutAsJsonAsync(Endpoint + "/0", newComment);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async void UpdateById_UserNotOwner_Forbidden()
    {
        // Arrange
        var comment = await Factory.DbManager.Seeder.SeedCommentAsync();
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest { UserName = user.UserName!, Password = user.UserName! };
        var newComment = CommentGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Account/Login", loginUser);
        using var secondResponse = await client.PutAsJsonAsync(Endpoint + $"/{comment.Id}", newComment);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}