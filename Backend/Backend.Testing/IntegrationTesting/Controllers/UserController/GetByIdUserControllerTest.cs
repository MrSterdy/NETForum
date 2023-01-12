﻿using System.Net;

using Backend.Core.Models.User;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.UserController;

public class GetByIdUserControllerTest : UserControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Id";

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
        using var response = await client.GetAsync(Endpoint + $"/{user.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var result = await ParseResponse<UserResponse>(response);
        result.UserName.Should().Be(user.UserName);
        result.Email.Should().Be(user.Email);
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