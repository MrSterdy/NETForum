using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.Auth;

using Bogus;
using Bogus.Extensions;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.AuthController;

public class LoginAuthControllerTest : AuthControllerTest
{
    private readonly Faker<AuthUser> _userGenerator = new Faker<AuthUser>()
        .RuleFor(u => u.UserName, faker => faker.Internet.UserName().ClampLength(4, 16))
        .RuleFor(u => u.Password, faker => faker.Internet.Password());

    protected override string Endpoint => base.Endpoint + "/Login";

    public LoginAuthControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void Login_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new AuthUser { UserName = user.UserName!, Password = user.UserName! };
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(Endpoint, loginUser);
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void Login_UserNotExists_BadRequest()
    {
        // Arrange
        var user = _userGenerator.Generate();

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(Endpoint, user);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async void Login_UserNotVerified_BadRequest()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedUserAsync();
        var loginUser = new AuthUser { UserName = user.UserName!, Password = user.UserName! };

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(Endpoint, loginUser);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async void Login_AlreadyLoggedIn_NotFound()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new AuthUser { UserName = user.UserName!, Password = user.UserName! };
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(Endpoint, loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, loginUser);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}