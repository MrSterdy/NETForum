using System.Net;

using NETForum.Models.Responses;

using FluentAssertions;

namespace NETForum.IntegrationTests.Controllers.UsersController;

public class GetByIdUsersControllerTest : UsersControllerTest
{
    public GetByIdUsersControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetById_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + $"/{user.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var result = await ParseResponse<UserResponse>(response);
        result.Id.Should().Be(user.Id);
        result.UserName.Should().Be(user.UserName);
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