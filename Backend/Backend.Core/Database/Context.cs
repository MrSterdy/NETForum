﻿using Backend.Core.Database.Entities;
using Backend.Core.Identity;
using Thread = Backend.Core.Database.Entities.Thread;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Backend.Core.Database;

public class Context : IdentityUserContext<ApplicationUser, int>
{
    public DbSet<Thread> Threads { get; set; } = default!;

    public DbSet<Comment> Comments { get; set; } = default!;

    public DbSet<IdentityRole<int>> Roles { get; set; } = default!;
    public DbSet<IdentityUserRole<int>> UserRoles { get; set; } = default!;

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .Ignore(u => u.PhoneNumber)
            .Ignore(u => u.PhoneNumberConfirmed)
            .Ignore(u => u.TwoFactorEnabled)
            .ToTable("Users");

        builder.Entity<IdentityRole<int>>()
            .ToTable("Roles");
        
        builder.Entity<IdentityRoleClaim<int>>()
            .ToTable("RoleClaims");
        
        builder.Entity<IdentityUserRole<int>>()
            .ToTable("UserRoles")
            .HasKey(r => new { r.UserId, r.RoleId });

        builder.Entity<IdentityUserClaim<int>>()
            .ToTable("Claims");

        builder.Ignore<IdentityUserToken<int>>();
        builder.Ignore<IdentityUserLogin<int>>();
    }
}