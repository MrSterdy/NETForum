﻿using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.User;
using Backend.Core.Models.User.Auth;

using Bogus;
using Bogus.Extensions;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.AuthController;

public class SignupAuthControllerTest : AuthControllerTest
{
    private readonly Faker<SignupUserRequest> _userGenerator = new Faker<SignupUserRequest>()
        .RuleFor(u => u.Email, faker => faker.Internet.Email())
        .RuleFor(u => u.UserName, faker => faker.Internet.UserName().ClampLength(4, 16))
        .RuleFor(u => u.Password, faker => faker.Internet.Password());

    protected override string Endpoint => base.Endpoint + "/Signup";

    public SignupAuthControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void Signup_Ok()
    {
        // Arrange
        var user = _userGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(Endpoint, user);
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<UserResponse>(response);

        result.Id.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async void Signup_InvalidModel_BadRequest()
    {
        // Arrange
        var user = _userGenerator.Generate();
        user.Email = "test";
        user.UserName = "";
        
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
        using var firstResponse = await client.PostAsJsonAsync(Endpoint, user);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, user);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
    [Fact]
    public async void Signup_AlreadyLoggedIn_NotFound()
    {
        // Arrange
        var createdUser = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest { UserName = createdUser.UserName!, Password = createdUser.UserName! };
        var signupUser = _userGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(base.Endpoint + "/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, signupUser);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}