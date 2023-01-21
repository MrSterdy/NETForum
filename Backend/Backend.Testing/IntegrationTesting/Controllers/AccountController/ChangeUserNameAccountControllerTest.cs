using System.Net.Http.Json;

using Backend.Core.Models.User.Account;

using Bogus;
using Bogus.Extensions;

namespace Backend.Testing.IntegrationTesting.Controllers.AccountController;

public class ChangeUserNameAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/UserName";

    private readonly Faker<ChangeUserNameRequest> _generator = new Faker<ChangeUserNameRequest>()
        .CustomInstantiator(faker => new ChangeUserNameRequest(faker.Internet.UserName().ClampLength(4, 16)));

    public ChangeUserNameAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void ChangeUserName_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var loginUser = new LoginRequest(user.UserName!, user.UserName!, true);
        var userNameRequest = _generator.Generate();

        // Act
        using var client = Factory.CreateClient();
        using var firstResponse = await client.PostAsJsonAsync(base.Endpoint + "/Login", loginUser);
        using var secondResponse = await client.PatchAsJsonAsync(Endpoint, userNameRequest);
        
        // Assert
        firstResponse.EnsureSuccessStatusCode();
        secondResponse.EnsureSuccessStatusCode();
    }
}