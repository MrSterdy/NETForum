using Backend.Testing.Database;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Backend.Testing.IntegrationTesting;

public class BackendFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public DbManager DbManager { get; private set; } = default!;
    
    public async Task InitializeAsync() =>
        await (DbManager = new DbManager()).InitializeAsync();

    public new async Task DisposeAsync() =>
        await DbManager.DisposeAsync();
}