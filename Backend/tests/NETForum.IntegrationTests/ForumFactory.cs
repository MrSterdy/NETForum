using NETForum.Infrastructure.Database;
using NETForum.Infrastructure.Mail;
using NETForum.IntegrationTests.Database;
using NETForum.IntegrationTests.Mail;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NETForum.IntegrationTests;

public class BackendFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string ConnectionString = "User ID=postgres;" +
                                            "Password=admin;" +
                                            "Database=test;" +
                                            "Host=localhost;" +
                                            "Port=5432;" +
                                            "Pooling=true;" +
                                            "Connection Lifetime=0;";
    
    public DbManager DbManager { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            services.Remove(dbDescriptor!);

            services.AddDbContext<ApplicationDbContext>(options => options
                .UseNpgsql(ConnectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            var mailDescriptor = new ServiceDescriptor(
                typeof(IMailService),
                typeof(FakeMailService),
                ServiceLifetime.Scoped
            );
            services.Replace(mailDescriptor);
        });
    }

    public async Task InitializeAsync() =>
        await (DbManager = new DbManager(ConnectionString)).InitializeAsync();

    public new async Task DisposeAsync() =>
        await DbManager.DisposeAsync();
}