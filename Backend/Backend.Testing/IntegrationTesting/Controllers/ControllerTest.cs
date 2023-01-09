using FluentAssertions;

using Newtonsoft.Json;

namespace Backend.Testing.IntegrationTesting.Controllers;

[Collection("Backend")]
public abstract class ControllerTest : IAsyncLifetime
{
    protected readonly BackendFactory Factory;

    protected abstract string Endpoint { get; }

    public ControllerTest(BackendFactory factory) =>
        Factory = factory;

    protected static async Task<T> ParseResponse<T>(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        body.Should().NotBeNullOrWhiteSpace();
        
        return JsonConvert.DeserializeObject<T>(body);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync() => await Factory.DbManager.ResetDatabaseAsync();
}