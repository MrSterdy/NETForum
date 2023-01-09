using Backend.Core.Database;

using Microsoft.EntityFrameworkCore;

namespace Backend.Testing.Database;

public class TestContext : Context
{
    public TestContext() : base(CreateOptions())
    {
    }

    private static DbContextOptions<Context> CreateOptions() =>
        new DbContextOptionsBuilder<Context>()
            .UseNpgsql("User ID=postgres;Password=admin;Host=localhost;Port=5432;Pooling=true;Connection Lifetime=0;")
            .Options;
}