using Thread = Backend.Database.Entities.Thread;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Backend.Database;

public class Context : IdentityUserContext<IdentityUser<int>, int>
{
    public DbSet<Thread> Threads { get; set; } = default!;

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityUser<int>>()
            .Ignore(u => u.PhoneNumber)
            .Ignore(u => u.PhoneNumberConfirmed)
            .Ignore(u => u.TwoFactorEnabled)
            .ToTable("Users");

        builder.Entity<IdentityUserClaim<int>>()
            .ToTable("Claims");

        builder.Ignore<IdentityUserToken<int>>();
        builder.Ignore<IdentityUserLogin<int>>();
    }
}