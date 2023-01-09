﻿using Microsoft.EntityFrameworkCore;

using Respawn;

namespace Backend.Testing.Database;

public class DbManager : IAsyncLifetime
{
    private TestContext Context { get; set; } = default!;
    private Respawner Respawner { get; set; } = default!;
    
    public Seeder Seeder { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await InitializeDbContextAsync();
        await InitializeRespawnerAsync();
        
        Seeder = new Seeder(Context);
    }
    
    private async Task InitializeDbContextAsync()
    {
        Context = new TestContext();

        await Context.Database.OpenConnectionAsync();
    }

    private async Task InitializeRespawnerAsync() =>
        Respawner = await Respawner.CreateAsync(
            Context.Database.GetDbConnection(),
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "public" }
            }
        );
    
    public async Task DisposeAsync() =>
        await Context.DisposeAsync();
    
    public async Task ResetDatabaseAsync() =>
        await Respawner.ResetAsync(Context.Database.GetDbConnection());
}