using Backend.Core.Models;
using Backend.Core.Models.Thread;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.ThreadController;

public class GetByUserIdThreadControllerTest : ThreadControllerTest
{
    protected override string Endpoint => base.Endpoint + "/User";

    public GetByUserIdThreadControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void GetByUserId_Ok()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + $"/{thread.UserId}?page=1");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<ThreadResponse>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().NotBeEmpty();
    }
    
    [Fact]
    public async void GetByUserId_InvalidUserId_Empty()
    {
        // Arrange
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + "/0");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<ThreadResponse>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().BeEmpty();
    }
}