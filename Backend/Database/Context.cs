using Backend.Database.Entities;
using Thread = Backend.Database.Entities.Thread;

using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public sealed class Context : DbContext
{
    public DbSet<Thread> Threads { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
}