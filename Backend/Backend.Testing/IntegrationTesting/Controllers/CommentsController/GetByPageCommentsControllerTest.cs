using Backend.Core.Models;
using Backend.Core.Models.Comment;
using Backend.Core.Models.Thread;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.CommentsController;

public class GetByPageCommentsControllerTest : CommentsControllerTest
{
    public GetByPageCommentsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void GetByPage_Ok()
    {
        // Arrange
        var comment = await Factory.DbManager.Seeder.SeedCommentAsync();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + $"?page=1&threadId={comment.ThreadId}");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<CommentResponse>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async void GetByPage_InvalidPageThread_Empty()
    {
        // Arrange
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + "?page=1&threadId=0");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<ThreadResponse>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().BeEmpty();
    }
}