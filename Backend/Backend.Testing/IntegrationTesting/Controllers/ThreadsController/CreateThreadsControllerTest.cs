using System.Net;
using System.Net.Http.Json;

using Backend.Core.Models.Thread;
using Backend.Core.Models.User.Auth;

using Bogus;

using FluentAssertions;

namespace Backend.Testing.IntegrationTesting.Controllers.ThreadsController;

public class CreateThreadsControllerTest : ThreadsControllerTest
{
    private readonly Faker<ThreadRequest> _threadGenerator = new Faker<ThreadRequest>()
        .CustomInstantiator(faker => new ThreadRequest(
            faker.Lorem.Sentence(),
            faker.Lorem.Paragraph()
        ));

    public CreateThreadsControllerTest(BackendFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void Create_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, true);
        var thread = _threadGenerator.Generate();
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Auth/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, thread);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async void Create_InvalidModel_BadRequest()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginUserRequest(user.UserName!, user.UserName!, true);
        var thread = new ThreadRequest("", "");
        
        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync("Api/Auth/Login", loginUser);
        using var secondResponse = await client.PostAsJsonAsync(Endpoint, thread);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}