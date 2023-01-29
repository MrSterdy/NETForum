using Microsoft.EntityFrameworkCore;

using NETForum.Infrastructure.Database;

using Respawn;

namespace NETForum.IntegrationTests.Database;

public class DbManager : IAsyncLifetime
{
    private readonly string _connectionString;
    
    private ApplicationDbContext Context { get; set; } = default!;
    private Respawner Respawner { get; set; } = default!;
    
    public Seeder Seeder { get; private set; } = default!;

    public DbManager(string connectionString) =>
        _connectionString = connectionString;

    public async Task InitializeAsync()
    {
        await InitializeDbContextAsync();
        await InitializeRespawnerAsync();

        Seeder = new Seeder(Context);
    }

    private async Task InitializeDbContextAsync()
    {
        Context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(_connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options);

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