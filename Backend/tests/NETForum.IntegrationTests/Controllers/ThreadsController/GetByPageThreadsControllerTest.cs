using FluentAssertions;

using NETForum.Infrastructure.Database.Entities;
using NETForum.Models.Responses;

namespace NETForum.IntegrationTests.Controllers.ThreadsController;

public class GetByPageThreadsControllerTest : ThreadsControllerTest
{
    public GetByPageThreadsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void GetByPage_WithoutUser_Ok()
    {
        // Arrange
        await Factory.DbManager.Seeder.SeedThreadAsync();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + "?page=1");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<ThreadResponse>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().NotBeEmpty();
    }
    
    [Fact]
    public async void GetByPage_WithUser_Ok()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        var userId = thread.UserId;
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + $"?page=1&userId={userId}");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<ThreadResponse>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().NotBeEmpty();
    }
    
    [Fact]
    public async void GetByPage_InvalidPageUser_Empty()
    {
        // Arrange
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + "?page=1&userId=0");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<ThreadResponse>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().BeEmpty();
    }
}