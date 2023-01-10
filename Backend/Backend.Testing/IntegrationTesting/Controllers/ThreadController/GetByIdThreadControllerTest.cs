using System.Net;

using Thread = Backend.Core.Database.Entities.Thread;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.ThreadController;

public class GetByIdThreadControllerTest : ThreadControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Id";

    public GetByIdThreadControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void GetById_Ok()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + $"/{thread.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Thread>(response);
        result.Title.Should().Be(thread.Title);
        result.Content.Should().Be(thread.Content);
        result.UserId.Should().Be(thread.UserId);
    }
    
    [Fact]
    public async void GetById_NotFound()
    {
        // Arrange

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + "/0");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}