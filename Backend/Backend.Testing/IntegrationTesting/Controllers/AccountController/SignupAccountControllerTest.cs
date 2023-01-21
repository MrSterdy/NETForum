﻿using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.User.Account;

using Bogus;
using Bogus.Extensions;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.AccountController;

public class SignupAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Signup";
    
    private readonly Faker<SignupRequest> _userGenerator = new Faker<SignupRequest>()
        .CustomInstantiator(faker => new SignupRequest(
            faker.Internet.Email(),
            faker.Internet.UserName().ClampLength(4, 16),
            faker.Internet.Password()
        ));

    public SignupAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void Signup_Ok()
    {
        // Arrange
        var user = _userGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(
            Endpoint + $"?callbackUrl={new Faker().Internet.Url()}",
            user
        );
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<AccountResponse>(response);
        result.Id.Should().BeGreaterThan(0);
        result.Email.Should().Be(user.Email);
        result.UserName.Should().Be(user.UserName);
        result.Confirmed.Should().BeFalse();
    }
    
    [Fact]
    public async void Signup_InvalidModel_BadRequest()
    {
        // Arrange
        var user = new SignupRequest("invalidemail", "verylonginvalidusername", "pw");

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(Endpoint, user);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async void Signup_UserAlreadyExists_Conflict()
    {
        // Arrange
        var user = _userGenerator.Generate();

        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(
            Endpoint + $"?callbackUrl={new Faker().Internet.Url()}",
            user
        );
        using var secondResponse = await client.PostAsJsonAsync(
            Endpoint + $"?callbackUrl={new Faker().Internet.Url()}",
            user
        );
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
    [Fact]
    public async void Signup_AlreadyLoggedIn_Forbidden()
    {
        // Arrange
        var createdUser = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest(createdUser.UserName!, createdUser.UserName!, true);
        var signupUser = _userGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(base.Endpoint + "/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(
            Endpoint + $"?callbackUrl={new Faker().Internet.Url()}",
            signupUser
        );
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}