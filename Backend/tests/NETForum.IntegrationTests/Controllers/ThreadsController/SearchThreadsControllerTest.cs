using NETForum.Infrastructure.Database.Entities;
using NETForum.Models.Responses;

using FluentAssertions;

namespace NETForum.IntegrationTests.Controllers.ThreadsController;

public class SearchThreadsControllerTest : ThreadsControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Search";

    public SearchThreadsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void Search_Ok()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        var search = thread.Title[..5];
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + $"?title={search}&page=1");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<ThreadResponse>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().NotBeEmpty();
    }
}