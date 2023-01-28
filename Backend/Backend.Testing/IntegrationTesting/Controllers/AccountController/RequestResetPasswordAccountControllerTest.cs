﻿using System.Net.Http.Json;

using Backend.Core.Models.User.Account;

using Bogus;

namespace Backend.Testing.IntegrationTesting.Controllers.AccountController;

public class RequestResetPasswordAccountControllerTest : AccountControllerTest
{
    protected override string Endpoint => base.Endpoint + "/ResetPassword";

    public RequestResetPasswordAccountControllerTest(BackendFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void RequestResetPassword_Ok()
    {
        // Arrange
        var user = await Factory.DbManager.Seeder.SeedVerifiedUserAsync();
        var emailRequest = new RequiredEmailRequest { Email = user.Email! };

        // Act
        using var client = Factory.CreateClient();
        using var response = await client.PostAsJsonAsync(
            Endpoint + $"?callbackUrl={new Faker().Internet.Url()}",
            emailRequest
        );
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}