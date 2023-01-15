﻿using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.User.Auth;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.CommentsController;

public class DeleteByIdCommentsControllerTest : CommentsControllerTest
{
    public DeleteByIdCommentsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void DeleteById_Ok()
    {
        // Arrange
        var comment = await Factory.DbManager.Seeder.SeedCommentAsync();
        var user = comment.User;
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, false);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Auth/Login", loginUser);
        using var secondResponse = await client.DeleteAsync(Endpoint + $"/{comment.Id}");
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void DeleteById_CommentNotExist_NotFound()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, false);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Auth/Login", loginUser);
        using var secondResponse = await client.DeleteAsync(Endpoint + "/0");
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void DeleteById_UserNotOwner_Forbidden()
    {
        // Arrange
        var comment = await Factory.DbManager.Seeder.SeedCommentAsync();
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, false);
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("/Api/Auth/Login", loginUser);
        using var secondResponse = await client.DeleteAsync(Endpoint + $"/{comment.Id}");
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}