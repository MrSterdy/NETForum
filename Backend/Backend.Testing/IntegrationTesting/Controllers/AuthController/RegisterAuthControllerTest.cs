using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models;
using Backend.Core.Models.Auth;

using Bogus;
using Bogus.Extensions;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.AuthController;

public class RegisterAuthControllerTest : AuthControllerTest
{
    private readonly Faker<RegisterUser> _userGenerator = new Faker<RegisterUser>()
        .RuleFor(u => u.Email, faker => faker.Internet.Email())
        .RuleFor(u => u.UserName, faker => faker.Internet.UserName().ClampLength(4, 16))
        .RuleFor(u => u.Password, faker => faker.Internet.Password());

    public RegisterAuthControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void Register_Ok()
    {
        // Arrange
        var user = _userGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(Endpoint + "/register", user);
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<User>(response);

        result.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async void Register_InvalidModel_BadRequest()
    {
        // Arrange
        var user = _userGenerator.Generate();
        user.Email = "dlwlwd";
        user.UserName = "";
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(Endpoint + "/register", user);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async void Register_UserAlreadyExists_Conflict()
    {
        // Arrange
        var user = _userGenerator.Generate();

        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(Endpoint + "/register", user);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint + "/register", user);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
    [Fact]
    public async void Register_AlreadyLoggedIn_NotFound()
    {
        // Arrange
        var createdUser = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new AuthUser { UserName = createdUser.UserName!, Password = createdUser.UserName! };
        var registerUser = _userGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(Endpoint + "/login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint + "/register", registerUser);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}