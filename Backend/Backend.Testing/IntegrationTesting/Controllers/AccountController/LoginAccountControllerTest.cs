using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.User.Account;

using Bogus;
using Bogus.Extensions;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.AccountController;

public class LoginAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Login";
    
    private readonly Faker<LoginRequest> _userGenerator = new Faker<LoginRequest>()
        .CustomInstantiator(faker => new LoginRequest(
            faker.Internet.UserName().ClampLength(4, 16),
            faker.Internet.Password(),
            true
        ));

    public LoginAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void Login_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest(user.UserName!, user.UserName!, true);
        
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
        var loginUser = new LoginRequest(user.UserName!, user.UserName!, true);

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
        var loginUser = new LoginRequest(user.UserName!, user.UserName!, true);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(Endpoint, loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, loginUser);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}