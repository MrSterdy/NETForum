using Backend.Core.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Test;

public class DatabaseHelper
{
    public static Context CreateContext()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();
        
        var builder = new DbContextOptionsBuilder<Context>();
        builder
            .UseInMemoryDatabase(databaseName: "TestInMemoryDb")
            .UseInternalServiceProvider(serviceProvider);

        return new Context(builder.Options);
    }
}