using System.Net;
using Backend.Core.Models;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.UserController;

public class GetByIdUserControllerTest : UserControllerTest
{
    public GetByIdUserControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void GetById_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + $"/id/{user.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var result = await ParseResponse<User>(response);
        result.UserName.Should().Be(user.UserName);
        result.Email.Should().Be(user.Email);
    }
    
    [Fact]
    public async void GetById_NotFound()
    {
        // Arrange

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + "/id/0");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}