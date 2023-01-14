using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.User;
using Backend.Core.Models.User.Auth;

using Bogus;
using Bogus.Extensions;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.AuthController;

public class LoginAuthControllerTest : AuthControllerTest
{
    private readonly Faker<LoginUserRequest> _userGenerator = new Faker<LoginUserRequest>()
        .CustomInstantiator(faker => new LoginUserRequest(
            faker.Internet.UserName().ClampLength(4, 16),
            faker.Internet.Password(),
            true
        ));

    protected override string Endpoint => base.Endpoint + "/Login";

    public LoginAuthControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void Login_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, true);
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(Endpoint, loginUser);
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<AccountResponse>(response);
        result.Id.Should().Be(user.Id);
        result.UserName.Should().Be(user.UserName);
        result.Email.Should().Be(user.Email);
        result.Confirmed.Should().BeTrue();
    }
    
    [Fact]
    public async void Login_UserNotExist_BadRequest()
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
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, true);

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(Endpoint, loginUser);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async void Login_AlreadyLoggedIn_Forbidden()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, true);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(Endpoint, loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, loginUser);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}