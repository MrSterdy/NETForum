using NETForum.Infrastructure.Database.Entities;
using NETForum.Models.Responses;

using FluentAssertions;

namespace NETForum.IntegrationTests.Controllers.ThreadsController;

public class SearchThreadsControllerTest : ThreadsControllerTest
{
    public SearchThreadsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void Search_User_Title_Page_Ok()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();

        var userId = thread.UserId;
        var title = thread.Title[..5];
        var page = 1;
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + $"?userId={userId}title={title}&page={page}");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<ThreadResponse>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().NotBeEmpty();
    }
}