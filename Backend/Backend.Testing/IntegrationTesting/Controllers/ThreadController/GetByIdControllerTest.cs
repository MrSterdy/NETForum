﻿using System.Net;

using Backend.Core.Database;
using Thread = Backend.Core.Database.Entities.Thread;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.ThreadController;

public class GetByIdControllerTest : ThreadControllerTest
{
    public GetByIdControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void GetById_Ok()
    {
        // Arrange
        var thread = await Factory.DbManager.Seeder.SeedThreadAsync();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + $"/id/{thread.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Thread>(response);
        result.Title.Should().Be(thread.Title);
        result.Content.Should().Be(thread.Content);
        result.UserId.Should().Be(thread.UserId);
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
    
    [Fact]
    public async void GetByPage_Ok()
    {
        // Arrange
        await Factory.DbManager.Seeder.SeedThreadAsync();
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + "/page/1");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<Thread>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().NotBeEmpty();
    }
    
    [Fact]
    public async void GetByPage_InvalidPage_Empty()
    {
        // Arrange
        
        // Act
        using var client = Factory.CreateClient();
        using var response = await client.GetAsync(Endpoint + "/page/0");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var result = await ParseResponse<Page<Thread>>(response);
        result.IsLast.Should().BeTrue();
        result.Items.Should().BeEmpty();
    }
}